using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	///<summary></summary>
	public class FormEtrans835ClaimPay : ODForm {
		private OpenDental.ValidDouble textWriteOff;
		private System.Windows.Forms.TextBox textInsPayAllowed;
		private System.Windows.Forms.TextBox textClaimFee;
		private System.Windows.Forms.Label label1;
		///<summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butDeductible;
		private OpenDental.UI.Button butWriteOff;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private OpenDental.UI.ODGrid gridPayments;
		private ODGrid gridClaimAdjustments;
		private ODGrid gridProcedureBreakdown;
		private UI.Button butViewEobDetails;
		private List<Procedure> _listProcs;
		private Patient _patCur;
		private Family _famCur;
		private List<InsPlan> _listPlans;
		private List<PatPlan> _listPatPlans;
		private List<InsSub> _listInsSubs;
		private X835 _x835;
		private Hx835_Claim _claimPaid;
		///<summary>All Hx835_Claims from _listOtherSplitClaims and claimPaid all in one list.</summary>
		private List<Hx835_Claim> _listAllClaimsPaid;
		private Claim _claim;
		private TextBox textDedApplied;
		private TextBox textInsPayAmt;
		private TextBox textEobInsPayAmt;
		private TextBox textEobDedApplied;
		private TextBox textEobInsPayAllowed;
		private TextBox textEobClaimFee;
		private Label label5;
		///<summary>The claim procs shown in the grid.  These procs are saved to/from the grid, but changes are not saved to the database unless the OK button is pressed or an individual claim proc is double-clicked for editing.</summary>
		public List <ClaimProc> ListClaimProcsForClaim;
		private List<ClaimProc> _listClaimProcsOld;
		private CheckBox checkIncludeWOPercCoPay;
		private InsPlan _insPlan;
		private UI.Button butSplitProcs;
		///<summary>Index of the Split column in the gridPayments.</summary>
		private int splitIndex=-1;
		///<summary>Index of the Deduct column in the gridPayments.</summary>
		private int deductIndex=-1;
		///<summary>Index of the Allowed column in the gridPayments.</summary>
		private int allowedIndex=-1;
		///<summary>Index of the InsPay/InsEst column in the gridPayments.</summary>
		private int insPayEstIndex=-1;
		///<summary>Index of the Writeoff column in the gridPayments.</summary>
		private int writeoffIndex=-1;
		///<summary>Index of the Remarks column in the gridPayments.</summary>
		private int remarkIndex=-1;

		///<summary>Flag used to for claim fee and fee billed validation logic.
		///Supplemental payments have a feeBilled of 0.
		///When validating a selection we compare the sum of all feeBilled to claimPaid.ClaimFee, both are set to 0 when true.</summary>
		private bool _isSupplementalPay;

		///<summary>The claimPaid is the individual EOB to load this window for.
		///The listOtherSplitClaims will contain any split claims which are associated to claimPaid.</summary>
		public FormEtrans835ClaimPay(X835 x835,Hx835_Claim claimPaid,Claim claim,Patient patCur,Family famCur,List<InsPlan> planList,List<PatPlan> patPlanList,List<InsSub> subList,bool isSupplementalPay) {
			InitializeComponent();
			_x835=x835;
			_claimPaid=claimPaid;
			_listAllClaimsPaid=new List<Hx835_Claim>() { _claimPaid };
			_listAllClaimsPaid.AddRange(_claimPaid.GetOtherNotDetachedSplitClaims());
			_claim=claim;
			_famCur=famCur;
			_patCur=patCur;
			_listPlans=planList;
			_listInsSubs=subList;
			_listPatPlans=patPlanList;
			//If the claim is already received, then the only way to enter payment on top of the existing is to use supplemental.
			_isSupplementalPay=isSupplementalPay;
			Lan.F(this);
		}

		///<summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components!=null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans835ClaimPay));
			this.textInsPayAllowed = new System.Windows.Forms.TextBox();
			this.textClaimFee = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.gridClaimAdjustments = new OpenDental.UI.ODGrid();
			this.gridProcedureBreakdown = new OpenDental.UI.ODGrid();
			this.gridPayments = new OpenDental.UI.ODGrid();
			this.textDedApplied = new System.Windows.Forms.TextBox();
			this.textInsPayAmt = new System.Windows.Forms.TextBox();
			this.textEobInsPayAmt = new System.Windows.Forms.TextBox();
			this.textEobDedApplied = new System.Windows.Forms.TextBox();
			this.textEobInsPayAllowed = new System.Windows.Forms.TextBox();
			this.textEobClaimFee = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.butViewEobDetails = new OpenDental.UI.Button();
			this.butWriteOff = new OpenDental.UI.Button();
			this.butDeductible = new OpenDental.UI.Button();
			this.textWriteOff = new OpenDental.ValidDouble();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.checkIncludeWOPercCoPay = new System.Windows.Forms.CheckBox();
			this.butSplitProcs = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// textInsPayAllowed
			// 
			this.textInsPayAllowed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textInsPayAllowed.Location = new System.Drawing.Point(485, 608);
			this.textInsPayAllowed.Name = "textInsPayAllowed";
			this.textInsPayAllowed.ReadOnly = true;
			this.textInsPayAllowed.Size = new System.Drawing.Size(62, 20);
			this.textInsPayAllowed.TabIndex = 116;
			this.textInsPayAllowed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textClaimFee
			// 
			this.textClaimFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textClaimFee.Location = new System.Drawing.Point(361, 608);
			this.textClaimFee.Name = "textClaimFee";
			this.textClaimFee.ReadOnly = true;
			this.textClaimFee.Size = new System.Drawing.Size(62, 20);
			this.textClaimFee.TabIndex = 118;
			this.textClaimFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(207, 611);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(150, 16);
			this.label1.TabIndex = 117;
			this.label1.Text = "Totals";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(496, 658);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(311, 39);
			this.label2.TabIndex = 122;
			this.label2.Text = "Before you click OK, the Deductible and the Ins Pay amounts should exactly match " +
    "the insurance EOB.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(20, 622);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 34);
			this.label3.TabIndex = 123;
			this.label3.Text = "Assign to selected payment line:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(164, 627);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(108, 29);
			this.label4.TabIndex = 124;
			this.label4.Text = "On all unpaid procedure amounts:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// gridClaimAdjustments
			// 
			this.gridClaimAdjustments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridClaimAdjustments.Location = new System.Drawing.Point(9, 12);
			this.gridClaimAdjustments.Name = "gridClaimAdjustments";
			this.gridClaimAdjustments.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridClaimAdjustments.Size = new System.Drawing.Size(956, 100);
			this.gridClaimAdjustments.TabIndex = 200;
			this.gridClaimAdjustments.TabStop = false;
			this.gridClaimAdjustments.Title = "EOB Claim Adjustments";
			this.gridClaimAdjustments.TranslationName = "FormEtrans835Edit";
			this.gridClaimAdjustments.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridClaimAdjustments_CellDoubleClick);
			// 
			// gridProcedureBreakdown
			// 
			this.gridProcedureBreakdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProcedureBreakdown.Location = new System.Drawing.Point(9, 118);
			this.gridProcedureBreakdown.Name = "gridProcedureBreakdown";
			this.gridProcedureBreakdown.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridProcedureBreakdown.Size = new System.Drawing.Size(956, 168);
			this.gridProcedureBreakdown.TabIndex = 199;
			this.gridProcedureBreakdown.TabStop = false;
			this.gridProcedureBreakdown.Title = "EOB Procedure Breakdown";
			this.gridProcedureBreakdown.TranslationName = "FormEtrans835Edit";
			this.gridProcedureBreakdown.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridProcedureBreakdown_CellDoubleClick);
			// 
			// gridPayments
			// 
			this.gridPayments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPayments.Location = new System.Drawing.Point(9, 345);
			this.gridPayments.Name = "gridPayments";
			this.gridPayments.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridPayments.Size = new System.Drawing.Size(956, 257);
			this.gridPayments.TabIndex = 125;
			this.gridPayments.Title = "Enter Payments";
			this.gridPayments.TranslationName = "TableClaimProc";
			this.gridPayments.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridPayments.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPayments_CellClick);
			this.gridPayments.CellTextChanged += new System.EventHandler(this.gridMain_CellTextChanged);
			// 
			// textDedApplied
			// 
			this.textDedApplied.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textDedApplied.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textDedApplied.Location = new System.Drawing.Point(423, 608);
			this.textDedApplied.Name = "textDedApplied";
			this.textDedApplied.ReadOnly = true;
			this.textDedApplied.Size = new System.Drawing.Size(62, 20);
			this.textDedApplied.TabIndex = 202;
			this.textDedApplied.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textInsPayAmt
			// 
			this.textInsPayAmt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textInsPayAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textInsPayAmt.Location = new System.Drawing.Point(547, 608);
			this.textInsPayAmt.Name = "textInsPayAmt";
			this.textInsPayAmt.ReadOnly = true;
			this.textInsPayAmt.Size = new System.Drawing.Size(62, 20);
			this.textInsPayAmt.TabIndex = 203;
			this.textInsPayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textEobInsPayAmt
			// 
			this.textEobInsPayAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textEobInsPayAmt.Location = new System.Drawing.Point(517, 292);
			this.textEobInsPayAmt.Name = "textEobInsPayAmt";
			this.textEobInsPayAmt.ReadOnly = true;
			this.textEobInsPayAmt.Size = new System.Drawing.Size(62, 20);
			this.textEobInsPayAmt.TabIndex = 209;
			this.textEobInsPayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textEobDedApplied
			// 
			this.textEobDedApplied.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textEobDedApplied.Location = new System.Drawing.Point(393, 292);
			this.textEobDedApplied.Name = "textEobDedApplied";
			this.textEobDedApplied.ReadOnly = true;
			this.textEobDedApplied.Size = new System.Drawing.Size(62, 20);
			this.textEobDedApplied.TabIndex = 208;
			this.textEobDedApplied.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textEobInsPayAllowed
			// 
			this.textEobInsPayAllowed.Location = new System.Drawing.Point(455, 292);
			this.textEobInsPayAllowed.Name = "textEobInsPayAllowed";
			this.textEobInsPayAllowed.ReadOnly = true;
			this.textEobInsPayAllowed.Size = new System.Drawing.Size(62, 20);
			this.textEobInsPayAllowed.TabIndex = 204;
			this.textEobInsPayAllowed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textEobClaimFee
			// 
			this.textEobClaimFee.Location = new System.Drawing.Point(331, 292);
			this.textEobClaimFee.Name = "textEobClaimFee";
			this.textEobClaimFee.ReadOnly = true;
			this.textEobClaimFee.Size = new System.Drawing.Size(62, 20);
			this.textEobClaimFee.TabIndex = 206;
			this.textEobClaimFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(177, 295);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(150, 16);
			this.label5.TabIndex = 205;
			this.label5.Text = "EOB Totals";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butViewEobDetails
			// 
			this.butViewEobDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butViewEobDetails.Location = new System.Drawing.Point(331, 659);
			this.butViewEobDetails.Name = "butViewEobDetails";
			this.butViewEobDetails.Size = new System.Drawing.Size(135, 25);
			this.butViewEobDetails.TabIndex = 201;
			this.butViewEobDetails.Text = "EOB Claim Details";
			this.butViewEobDetails.Click += new System.EventHandler(this.butViewEobDetails_Click);
			// 
			// butWriteOff
			// 
			this.butWriteOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butWriteOff.Location = new System.Drawing.Point(163, 659);
			this.butWriteOff.Name = "butWriteOff";
			this.butWriteOff.Size = new System.Drawing.Size(90, 25);
			this.butWriteOff.TabIndex = 121;
			this.butWriteOff.Text = "&Write Off";
			this.butWriteOff.Click += new System.EventHandler(this.butWriteOff_Click);
			// 
			// butDeductible
			// 
			this.butDeductible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDeductible.Location = new System.Drawing.Point(23, 659);
			this.butDeductible.Name = "butDeductible";
			this.butDeductible.Size = new System.Drawing.Size(92, 25);
			this.butDeductible.TabIndex = 120;
			this.butDeductible.Text = "&Deductible";
			this.butDeductible.Click += new System.EventHandler(this.butDeductible_Click);
			// 
			// textWriteOff
			// 
			this.textWriteOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textWriteOff.Location = new System.Drawing.Point(609, 608);
			this.textWriteOff.MaxVal = 100000000D;
			this.textWriteOff.MinVal = -100000000D;
			this.textWriteOff.Name = "textWriteOff";
			this.textWriteOff.ReadOnly = true;
			this.textWriteOff.Size = new System.Drawing.Size(62, 20);
			this.textWriteOff.TabIndex = 119;
			this.textWriteOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(890, 659);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(809, 659);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// checkIncludeWOPercCoPay
			// 
			this.checkIncludeWOPercCoPay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkIncludeWOPercCoPay.Location = new System.Drawing.Point(9, 322);
			this.checkIncludeWOPercCoPay.Name = "checkIncludeWOPercCoPay";
			this.checkIncludeWOPercCoPay.Size = new System.Drawing.Size(570, 19);
			this.checkIncludeWOPercCoPay.TabIndex = 210;
			this.checkIncludeWOPercCoPay.Text = "Include WriteOffs for Category Percentage and Medicaid/Flat CoPay plans";
			this.checkIncludeWOPercCoPay.UseVisualStyleBackColor = true;
			this.checkIncludeWOPercCoPay.CheckedChanged += new System.EventHandler(this.checkIncludeWOPercCoPay_CheckedChanged);
			// 
			// butSplitProcs
			// 
			this.butSplitProcs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSplitProcs.Location = new System.Drawing.Point(868, 318);
			this.butSplitProcs.Name = "butSplitProcs";
			this.butSplitProcs.Size = new System.Drawing.Size(97, 25);
			this.butSplitProcs.TabIndex = 210;
			this.butSplitProcs.Text = "Split Claim";
			this.butSplitProcs.Click += new System.EventHandler(this.butSplitProcs_Click);
			// 
			// FormEtrans835ClaimPay
			// 
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.checkIncludeWOPercCoPay);
			this.Controls.Add(this.butSplitProcs);
			this.Controls.Add(this.textEobInsPayAmt);
			this.Controls.Add(this.textEobDedApplied);
			this.Controls.Add(this.textEobInsPayAllowed);
			this.Controls.Add(this.textEobClaimFee);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textInsPayAmt);
			this.Controls.Add(this.textDedApplied);
			this.Controls.Add(this.butViewEobDetails);
			this.Controls.Add(this.gridClaimAdjustments);
			this.Controls.Add(this.gridProcedureBreakdown);
			this.Controls.Add(this.gridPayments);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butWriteOff);
			this.Controls.Add(this.butDeductible);
			this.Controls.Add(this.textWriteOff);
			this.Controls.Add(this.textInsPayAllowed);
			this.Controls.Add(this.textClaimFee);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(990, 734);
			this.Name = "FormEtrans835ClaimPay";
			this.ShowInTaskbar = false;
			this.Text = " Verify and Enter Payment";
			this.Load += new System.EventHandler(this.FormEtrans835ClaimPay_Load);
			this.Shown += new System.EventHandler(this.FormEtrans835ClaimPay_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEtrans835ClaimPay_Load(object sender, System.EventArgs e) {
			_listClaimProcsOld=new List<ClaimProc>();
			foreach(ClaimProc cp in ListClaimProcsForClaim) {
				_listClaimProcsOld.Add(cp.Copy());
			}
			_listProcs=Procedures.Refresh(_patCur.PatNum);
			FillGridClaimAdjustments();
			FillGridProcedureBreakdown();
			decimal claimfee=_listAllClaimsPaid.Sum(x => x.ClaimFee);
			if(_isSupplementalPay) {
				//Supplemental payments do not need to validate the claimFee matches the sum of feeBilleds.
				//So we set both the claimFee and all feeBilled values to 0.
				//This mimics how supplemental payments show in FormClaimEdit.
				claimfee=0;
			}
			textEobClaimFee.Text=claimfee.ToString("F");
			textEobDedApplied.Text=_listAllClaimsPaid.Sum(x => x.PatientDeductAmt).ToString("F");
			textEobInsPayAllowed.Text=_listAllClaimsPaid.Sum(x => x.AllowedAmt).ToString("F");
			textEobInsPayAmt.Text=_listAllClaimsPaid.Sum(x => x.InsPaid).ToString("F");
			_insPlan=InsPlans.RefreshOne(_claim.PlanNum);
			if(_insPlan!=null && _insPlan.PlanType.In("","f")) {
				checkIncludeWOPercCoPay.Checked=PrefC.GetBool(PrefName.EraIncludeWOPercCoPay);
			}
			else {
				checkIncludeWOPercCoPay.Checked=true;//Current behaviour is to include all WOs regardless of plan type.
				checkIncludeWOPercCoPay.Visible=false;
			}
			FillGridProcedures();
		}

		private void FormEtrans835ClaimPay_Shown(object sender,EventArgs e) {
			InsPlan plan=InsPlans.GetPlan(ListClaimProcsForClaim[0].PlanNum,_listPlans);
			int selectedIndex=0;
			if(_claimPaid.IsSplitClaim) {
				//For split claims we show all the procs on a claim 
				//but make rows bold if they are associated to this split claims procs.
				//Auto select the first bold row.
				selectedIndex=0;
				for(int i=0;i<gridPayments.ListGridRows.Count;i++){
					if(gridPayments.ListGridRows[i].Bold){
						selectedIndex=i;
						break;
					}
				}
			}
			if(plan.AllowedFeeSched!=0){//allowed fee sched
				gridPayments.SetSelected(new Point(7,selectedIndex));//Allowed column of the selected row.
			}
			else{
				gridPayments.SetSelected(new Point(8,selectedIndex));//InsPay column of the selected row.
			}
			HighlightEraProcRow(selectedIndex);
		}

		private void FillGridClaimAdjustments() {
			if(_listAllClaimsPaid.All(x => x.ListClaimAdjustments.Count==0)) {
				gridClaimAdjustments.Title="EOB Claim Adjustments (None Reported)";
			}
			else {
				gridClaimAdjustments.Title="EOB Claim Adjustments";
			}
			gridClaimAdjustments.BeginUpdate();
			gridClaimAdjustments.ListGridColumns.Clear();
			gridClaimAdjustments.ListGridColumns.Add(new UI.GridColumn("Reason",445,HorizontalAlignment.Left));
			gridClaimAdjustments.ListGridColumns.Add(new UI.GridColumn("Allowed",62,HorizontalAlignment.Right));
			gridClaimAdjustments.ListGridColumns.Add(new UI.GridColumn("Ins Pay",62,HorizontalAlignment.Right));
			gridClaimAdjustments.ListGridColumns.Add(new UI.GridColumn("Remarks",0,HorizontalAlignment.Left));
			gridClaimAdjustments.ListGridRows.Clear();
			foreach(Hx835_Adj adj in _listAllClaimsPaid.SelectMany(x => x.ListClaimAdjustments).ToList()) { 
				GridRow row=new GridRow();
				row.Tag=adj;
				row.Cells.Add(new GridCell(adj.ReasonDescript));//Reason
				row.Cells.Add(new GridCell((-adj.AdjAmt).ToString("f2")));//Allowed
				row.Cells.Add(new GridCell((-adj.AdjAmt).ToString("f2")));//Ins Pay
				row.Cells.Add(new GridCell(adj.AdjustRemarks));//Remarks
				gridClaimAdjustments.ListGridRows.Add(row);
			}
			gridClaimAdjustments.EndUpdate();
		}

		private void FillGridProcedureBreakdown() {
			if(_listAllClaimsPaid.All(x => x.ListProcs.Count==0)) {
				gridProcedureBreakdown.Title="EOB Procedure Breakdown (None Reported)";
			}
			else {
				gridProcedureBreakdown.Title="EOB Procedure Breakdown";
			}
			gridProcedureBreakdown.BeginUpdate();
			gridProcedureBreakdown.ListGridColumns.Clear();
			gridProcedureBreakdown.ListGridColumns.Add(new GridColumn("ProcNum",116,HorizontalAlignment.Left));
			gridProcedureBreakdown.ListGridColumns.Add(new GridColumn("Code",50,HorizontalAlignment.Center));
			gridProcedureBreakdown.ListGridColumns.Add(new GridColumn("",25,HorizontalAlignment.Center));
			gridProcedureBreakdown.ListGridColumns.Add(new GridColumn("Description",130,HorizontalAlignment.Left));
			gridProcedureBreakdown.ListGridColumns.Add(new GridColumn("Fee Billed",62,HorizontalAlignment.Right));
			gridProcedureBreakdown.ListGridColumns.Add(new GridColumn("Deduct",62,HorizontalAlignment.Right));
			gridProcedureBreakdown.ListGridColumns.Add(new GridColumn("Allowed",62,HorizontalAlignment.Right));
			string insTitle="InsPay";
			if(_claimPaid.IsPreauth) {
				insTitle="InsEst";
			}
			gridProcedureBreakdown.ListGridColumns.Add(new GridColumn(insTitle,62,HorizontalAlignment.Right));
			gridProcedureBreakdown.ListGridColumns.Add(new GridColumn("Remarks",0,HorizontalAlignment.Left));
			gridProcedureBreakdown.ListGridRows.Clear();
			foreach(Hx835_Proc proc in _listAllClaimsPaid.SelectMany(x => x.ListProcs)) {
				GridRow row=new GridRow();
				row.Tag=proc;
				if(proc.ProcNum==0) {
					row.Cells.Add(new GridCell(""));//ProcNum
				}
				else {
					row.Cells.Add(new GridCell(proc.ProcNum.ToString()));//ProcNum
				}
				row.Cells.Add(new GridCell(proc.ProcCodeAdjudicated));//Code
				row.Cells.Add(new GridCell(""));//Blank
				string procDescript="";
				if(ProcedureCodes.IsValidCode(proc.ProcCodeAdjudicated)) {
					ProcedureCode procCode=ProcedureCodes.GetProcCode(proc.ProcCodeAdjudicated);
					procDescript=procCode.AbbrDesc;
				}
				row.Cells.Add(new GridCell(procDescript));//Description
				decimal procFee=proc.ProcFee;
				if(_isSupplementalPay) {
					//Supplemental payments do not have a proc fee or fee billed.
					//Supplemental payments do not need to validate the claimFee matches the sum of feeBilleds
					procFee=0;
				}
				row.Cells.Add(new GridCell(procFee.ToString("f2")));//Fee Billed
				row.Cells.Add(new GridCell(proc.DeductibleAmt.ToString("f2")));//Deduct
				row.Cells.Add(new GridCell(proc.AllowedAmt.ToString("f2")));//Allowed
				row.Cells.Add(new GridCell(proc.InsPaid.ToString("f2")));//InsPay or InsEst
				row.Cells.Add(new GridCell(proc.GetRemarks()));//Remarks
				gridProcedureBreakdown.ListGridRows.Add(row);
			}
			gridProcedureBreakdown.EndUpdate();
		}

		private void FillGridProcedures(){
			bool isWOIncluded=checkIncludeWOPercCoPay.Checked;
			//Changes made in this window do not get saved until after this window closes.
			//But if you double click on a row, then you will end up saving.  That shouldn't hurt anything, but could be improved.
			//also calculates totals for this "payment"
			//the payment itself is imaginary and is simply the sum of the claimprocs on this form
			gridPayments.BeginUpdate();
			gridPayments.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.g("TableClaimProc","Split"),30,HorizontalAlignment.Center);//Split proc selection column
			gridPayments.ListGridColumns.Add(col);
			splitIndex=gridPayments.ListGridColumns.Count-1;
			col=new GridColumn(Lan.g("TableClaimProc","Date"),66);
			gridPayments.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableClaimProc","Prov"),50);
			gridPayments.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableClaimProc","Code"),50);
			gridPayments.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableClaimProc","Tth"),25);
			gridPayments.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableClaimProc","Description"),130);
			gridPayments.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableClaimProc","Fee Billed"),62,HorizontalAlignment.Right);
			gridPayments.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableClaimProc","Deduct"),62,HorizontalAlignment.Right,true);
			gridPayments.ListGridColumns.Add(col);
			deductIndex=gridPayments.ListGridColumns.Count-1;
			col=new GridColumn(Lan.g("TableClaimProc","Allowed"),62,HorizontalAlignment.Right,true);
			gridPayments.ListGridColumns.Add(col);
			allowedIndex=gridPayments.ListGridColumns.Count-1;
			string insTitle="InsPay";
			if(_claimPaid.IsPreauth) {
				insTitle="InsEst";
			}
			col=new GridColumn(Lan.g("TableClaimProc",insTitle),62,HorizontalAlignment.Right,true);
			gridPayments.ListGridColumns.Add(col);
			insPayEstIndex=gridPayments.ListGridColumns.Count-1;
			col=new GridColumn(Lan.g("TableClaimProc","Writeoff"),62,HorizontalAlignment.Right,isWOIncluded);
			gridPayments.ListGridColumns.Add(col);
			writeoffIndex=gridPayments.ListGridColumns.Count-1;
			col=new GridColumn(Lan.g("TableClaimProc","Status"),50,HorizontalAlignment.Center);
			gridPayments.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableClaimProc","Pmt"),30,HorizontalAlignment.Center);
			gridPayments.ListGridColumns.Add(col);
			col=new GridColumn(Lan.g("TableClaimProc","Remarks"),0,true);
			gridPayments.ListGridColumns.Add(col);
			remarkIndex=gridPayments.ListGridColumns.Count-1;
			gridPayments.ListGridRows.Clear();
			GridRow row;
			Procedure ProcCur;
			for(int i=0;i<ListClaimProcsForClaim.Count;i++){
				ClaimProc claimProc=ListClaimProcsForClaim[i];
				if((_isSupplementalPay && claimProc.Status==ClaimProcStatus.Received && claimProc.ProcNum!=0)//Skip original claims recieved claimProcs.
					|| (_isSupplementalPay && claimProc.Status==ClaimProcStatus.Received && claimProc.ProcNum==0 && !claimProc.IsNew)//Skip pre-exiting "By Total" payment claimProcs.
					|| (_isSupplementalPay && claimProc.Status==ClaimProcStatus.Supplemental && !claimProc.IsNew))//Skip pre-existing supplemental payments.
				{
					//When entering supplemental payments we only want to show newly created supplemental claimProcs or "By Total" claimProcs.
					continue;
				}
				row=new GridRow();
				row.Tag=claimProc;
				if(_claimPaid.IsSplitClaim
					&& claimProc.Status.In(ClaimProcStatus.Received,ClaimProcStatus.Supplemental)
					&& claimProc.IsNew)
				{
					//Highlight the subset of procdures for a split claim that we are entering payment for.
					//Split claims process a sub set of a claims procs. We show all procs here even if this split claim does not contain them.
					row.Bold=true;
				}
				row.Cells.Add("");//Split claim selection column
				if(claimProc.ProcNum==0) {//Total payment
					//We want to always show the "Payment Date" instead of the procedure date for total payments because they are not associated to procedures.
					row.Cells.Add(claimProc.DateCP.ToShortDateString());
				}
				else {
					row.Cells.Add(claimProc.ProcDate.ToShortDateString());
				}
				row.Cells.Add(Providers.GetAbbr(claimProc.ProvNum));
				if(claimProc.ProcNum==0) {
					row.Cells.Add("");
					row.Cells.Add("");
					row.Cells.Add(Lan.g(this,"Total Payment"));
				}
				else {
					ProcCur=Procedures.GetProcFromList(_listProcs,claimProc.ProcNum);
					row.Cells.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProcCode);
					row.Cells.Add(Tooth.ToInternat(ProcCur.ToothNum));
					row.Cells.Add(ProcedureCodes.GetProcCode(ProcCur.CodeNum).Descript);
				}
				row.Cells.Add(claimProc.FeeBilled.ToString("F"));
				row.Cells.Add(claimProc.DedApplied.ToString("F"));
				if(claimProc.AllowedOverride==-1){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(claimProc.AllowedOverride.ToString("F"));
				}
				if(claimProc.Status==ClaimProcStatus.Preauth) {
					row.Cells.Add(claimProc.InsPayEst.ToString("F"));
				}
				else {
					row.Cells.Add(claimProc.InsPayAmt.ToString("F"));
				}
				if(isWOIncluded) {
					row.Cells.Add(claimProc.WriteOff.ToString("F"));
				}
				else{
					row.Cells.Add(0d.ToString("F"));
				}
				switch(claimProc.Status){
					case ClaimProcStatus.Received:
						row.Cells.Add("Recd");
						break;
					case ClaimProcStatus.NotReceived:
						row.Cells.Add("");
						break;
					//adjustment would never show here
					case ClaimProcStatus.Preauth:
						row.Cells.Add("PreA");
						break;
					case ClaimProcStatus.Supplemental:
						row.Cells.Add("Supp");
						break;
					case ClaimProcStatus.CapClaim:
						row.Cells.Add("Cap");
						break;
					//Estimate would never show here
					//Cap would never show here
				}
				if(claimProc.ClaimPaymentNum>0){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(claimProc.Remarks);
				gridPayments.ListGridRows.Add(row);
			}
			gridPayments.EndUpdate();
			FillTotals();
		}

		private void gridClaimAdjustments_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Adj adj=(Hx835_Adj)gridClaimAdjustments.ListGridRows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(adj.AdjCode+" "+adj.AdjustRemarks+"\r\r"+adj.ReasonDescript+"\r\n"+adj.AdjAmt.ToString("f2"));
			msgbox.Show(this);//This window is just used to display information.
		}

		private void gridProcedureBreakdown_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Proc proc=(Hx835_Proc)gridProcedureBreakdown.ListGridRows[e.Row].Tag;
			FormEtrans835ProcEdit Form=new FormEtrans835ProcEdit(proc);
			Form.Show(this);//This window is just used to display information.
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			try{
				SaveGridChanges();
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			List<ClaimProcHist> histList=null;
			List<ClaimProcHist> loopList=null;
			ClaimProc claimProc=(ClaimProc)gridPayments.ListGridRows[e.Row].Tag;
			FormClaimProc FormCP=new FormClaimProc(claimProc,null,_famCur,_patCur,_listPlans,histList,ref loopList,_listPatPlans,false,_listInsSubs);
			FormCP.IsInClaim=true;
			//no need to worry about permissions here
			FormCP.ShowDialog();//Modal because this window can change information.
			if(FormCP.DialogResult!=DialogResult.OK){
				return;
			}
			if(claimProc.DoDelete) {
				ListClaimProcsForClaim.Remove(claimProc);
			}
			FillGridProcedures();
			FillTotals();
		}

		private void gridMain_CellTextChanged(object sender,EventArgs e) {
			FillTotals();
		}
		
		private void gridPayments_CellClick(object sender,ODGridClickEventArgs e) {
			HighlightEraProcRow(e.Row);
			switch(e.Col) {
				case 0://Split claim column
					bool isSpliting=false;
					if(gridPayments.ListGridRows[e.Row].Cells[e.Col].Text=="") {//Row is not selected to split currently, will select for split.
						isSpliting=true;
					}
					gridPayments.BeginUpdate();
					gridPayments.ListGridRows[e.Row].Cells[e.Col].Text=(isSpliting?"X":"");
					gridPayments.EndUpdate();
					break;
				case 10://WO column and not including WOs, so do not let them edit.
					if(!checkIncludeWOPercCoPay.Checked) {
						MsgBox.Show(this,"WriteOffs cannot be edited when Include Writeoffs checkbox is not checked.");
					}
					break;
			}
		}

		private void HighlightEraProcRow(int gridPaymentRowIndex) {
			ClaimProc selectedClaimProc=(ClaimProc)gridPayments.ListGridRows[gridPaymentRowIndex].Tag;
			if(selectedClaimProc.ProcNum==0) {
				return;
			}
			List<ClaimProc> listClaimProcsToMatch=new List<ClaimProc>() { selectedClaimProc };
			for(int i=0;i<gridProcedureBreakdown.ListGridRows.Count;i++){
				Hx835_Proc eraProc=(Hx835_Proc)gridProcedureBreakdown.ListGridRows[i].Tag;
				ClaimProc matchedClaimProc;
				eraProc.TryGetMatchedClaimProc(out matchedClaimProc,listClaimProcsToMatch,_isSupplementalPay);
				gridProcedureBreakdown.SetSelected(i,(matchedClaimProc!=null));
			}
		}

		///<Summary>Fails silently if text is in invalid format.</Summary>
		private void FillTotals(){
			double claimFee=0;
			double dedApplied=0;
			double insPayAmtAllowed=0;
			double insPayAmt=0;
			double writeOff=0;
			//double amt;
			for(int i=0;i<gridPayments.ListGridRows.Count;i++){
				ClaimProc claimProc=(ClaimProc)gridPayments.ListGridRows[i].Tag;
				claimFee+=claimProc.FeeBilled;//5
				dedApplied+=PIn.Double(gridPayments.ListGridRows[i].Cells[deductIndex].Text);//6.deduct
				insPayAmtAllowed+=PIn.Double(gridPayments.ListGridRows[i].Cells[allowedIndex].Text);//7.allowed
				insPayAmt+=PIn.Double(gridPayments.ListGridRows[i].Cells[insPayEstIndex].Text);//8.inspayest
				writeOff+=PIn.Double(gridPayments.ListGridRows[i].Cells[writeoffIndex].Text);//9.writeoff
			}
			textClaimFee.Text=claimFee.ToString("F");
			textDedApplied.Text=dedApplied.ToString("F");
			textInsPayAllowed.Text=insPayAmtAllowed.ToString("F");
			textInsPayAmt.Text=insPayAmt.ToString("F");
			textWriteOff.Text=writeOff.ToString("F");
		}

		///<Summary>Surround with try-catch.</Summary>
		private void SaveGridChanges(){
			//validate all grid cells
			double dbl;
			for(int i=0;i<gridPayments.ListGridRows.Count;i++){
				if(gridPayments.ListGridRows[i].Cells[deductIndex].Text!=""){//deduct
					try{
						dbl=Convert.ToDouble(gridPayments.ListGridRows[i].Cells[deductIndex].Text);
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Deductible not valid: ")+gridPayments.ListGridRows[i].Cells[deductIndex].Text);
					}
				}
				if(gridPayments.ListGridRows[i].Cells[allowedIndex].Text!=""){//allowed
					try{
						dbl=Convert.ToDouble(gridPayments.ListGridRows[i].Cells[allowedIndex].Text);
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Allowed amt not valid: ")+gridPayments.ListGridRows[i].Cells[allowedIndex].Text);
					}
				}
				if(gridPayments.ListGridRows[i].Cells[insPayEstIndex].Text!=""){//inspay
					try{
						dbl=Convert.ToDouble(gridPayments.ListGridRows[i].Cells[insPayEstIndex].Text);
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Ins Pay not valid: ")+gridPayments.ListGridRows[i].Cells[insPayEstIndex].Text);
					}
				}
				if(gridPayments.ListGridRows[i].Cells[writeoffIndex].Text!=""){//writeoff
					try{
						dbl=Convert.ToDouble(gridPayments.ListGridRows[i].Cells[writeoffIndex].Text);
						if(dbl<0 && !_claimPaid.IsReversal){//Claim reversals have negative writeoffs.
							throw new ApplicationException(Lan.g(this,"Writeoff cannot be negative: ")+gridPayments.ListGridRows[i].Cells[writeoffIndex].Text);
						}
					}
					catch{
						throw new ApplicationException(Lan.g(this,"Writeoff not valid: ")+gridPayments.ListGridRows[i].Cells[writeoffIndex].Text);
					}
				}
			}
			foreach(GridRow row in gridPayments.ListGridRows) {
				ClaimProc claimProc=(ClaimProc)row.Tag;
				claimProc.DedApplied=PIn.Double(row.Cells[deductIndex].Text);
				if(row.Cells[allowedIndex].Text==""){
					claimProc.AllowedOverride=-1;
				}
				else{
					claimProc.AllowedOverride=PIn.Double(row.Cells[allowedIndex].Text);
				}
				if(claimProc.Status==ClaimProcStatus.Preauth) {
					claimProc.InsPayEst=PIn.Double(row.Cells[insPayEstIndex].Text);
				}
				else {
					claimProc.InsPayAmt=PIn.Double(row.Cells[insPayEstIndex].Text);
				}
				claimProc.WriteOff=PIn.Double(row.Cells[writeoffIndex].Text);
				claimProc.Remarks=row.Cells[remarkIndex].Text;
			}
		}

		private void butDeductible_Click(object sender, System.EventArgs e) {
			if(gridPayments.SelectedCell.X==-1) {
				MessageBox.Show(Lan.g(this,"Please select one payment line.  Then click this button to assign the deductible to that line."));
				return;
			}
			try {
				SaveGridChanges();
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			Double dedAmt=0;
			ClaimProc claimProc;
			//remove the existing deductible from each payment line and move it to dedAmt.
			for(int i=0;i<gridPayments.ListGridRows.Count;i++) {
				claimProc=(ClaimProc)gridPayments.ListGridRows[i].Tag;
				if(claimProc.DedApplied > 0){
					dedAmt+=claimProc.DedApplied;
					claimProc.InsPayEst+=claimProc.DedApplied;//dedAmt might be more
					claimProc.InsPayAmt+=claimProc.DedApplied;
					claimProc.DedApplied=0;
				}
			}
			if(dedAmt==0){
				MessageBox.Show(Lan.g(this,"There does not seem to be a deductible to apply.  You can still apply a deductible manually by double clicking on a payment line."));
				return;
			}
			//then move dedAmt to the selected proc
			claimProc=(ClaimProc)gridPayments.ListGridRows[gridPayments.SelectedCell.Y].Tag;
			claimProc.DedApplied=dedAmt;
			claimProc.InsPayEst-=dedAmt;
			claimProc.InsPayAmt-=dedAmt;
			FillGridProcedures();
		}

		private void butWriteOff_Click(object sender, System.EventArgs e) {
			if(MessageBox.Show(Lan.g(this,"Write off unpaid amount on each procedure?"),""
				,MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			try {
				SaveGridChanges();
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			//fix later: does not take into account other payments.
			double unpaidAmt=0;
			List<Procedure> ProcList=Procedures.Refresh(_patCur.PatNum);
			for(int i=0;i<gridPayments.ListGridRows.Count;i++) {
				ClaimProc claimProc=(ClaimProc)gridPayments.ListGridRows[i].Tag;
				if(claimProc.ProcNum==0) {
					continue;//Ignore "Total Payment" lines.
				}
				unpaidAmt=Procedures.GetProcFromList(ProcList,claimProc.ProcNum).ProcFee
					//((Procedure)Procedures.HList[ClaimProcsToEdit[i].ProcNum]).ProcFee
					-claimProc.DedApplied
					-claimProc.InsPayAmt;
				if(unpaidAmt > 0){
					claimProc.WriteOff=unpaidAmt;
				}
			}
			FillGridProcedures();
		}

		private void SaveAllowedFees(){
			//if no allowed fees entered, then nothing to do 
			bool allowedFeesEntered=false;
			for(int i=0;i<gridPayments.ListGridRows.Count;i++){
				if(gridPayments.ListGridRows[i].Cells[allowedIndex].Text!=""){
					allowedFeesEntered=true;
					break;
				}
			}
			if(!allowedFeesEntered){
				return;
			}
			//if no allowed fee schedule, then nothing to do
			InsPlan plan=InsPlans.GetPlan(ListClaimProcsForClaim[0].PlanNum,_listPlans);
			if(plan.AllowedFeeSched==0){//no allowed fee sched
				//plan.PlanType!="p" && //not ppo, and 
				return;
			}
			//ask user if they want to save the fees
			if(!MsgBox.Show(this,true,"Save the allowed amounts to the allowed fee schedule?")){
				return;
			}
			//select the feeSchedule
			long feeSched=-1;
			feeSched=plan.AllowedFeeSched;
			if(FeeScheds.GetIsHidden(feeSched)){
				MsgBox.Show(this,"Allowed fee schedule is hidden, so no changes can be made.");
				return;
			}
			Fee feeCur=null;
			long codeNum;
			List<Procedure> listProcs=Procedures.Refresh(_patCur.PatNum);
			Procedure proc;
			List<long> invalidFeeSchedNums = new List<long>();
			for(int i=0;i<gridPayments.ListGridRows.Count;i++) {
				ClaimProc claimProc=(ClaimProc)gridPayments.ListGridRows[i].Tag;
				proc=Procedures.GetProcFromList(listProcs,claimProc.ProcNum);
				codeNum=proc.CodeNum;
				//skip total payments
				if(codeNum==0){
					continue;
				}
				feeCur=Fees.GetFee(codeNum,feeSched,proc.ClinicNum,proc.ProvNum);
				DateTime datePrevious=DateTime.MinValue;
				if(feeCur==null){
					feeCur=new Fee();
					feeCur.FeeSched=feeSched;
					feeCur.CodeNum=codeNum;
					feeCur.ClinicNum=(FeeScheds.GetFirst(x => x.FeeSchedNum==feeSched).IsGlobal) ? 0 : proc.ClinicNum;
					feeCur.ProvNum=(FeeScheds.GetFirst(x => x.FeeSchedNum==feeSched).IsGlobal) ? 0 : proc.ProvNum;
					feeCur.Amount=PIn.Double(gridPayments.ListGridRows[i].Cells[allowedIndex].Text);
					Fees.Insert(feeCur);
				}
				else{
					feeCur.Amount=PIn.Double(gridPayments.ListGridRows[i].Cells[allowedIndex].Text);
					datePrevious=feeCur.SecDateTEdit;
					Fees.Update(feeCur);
				}
				SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,Lan.g(this,"Procedure")+": "+ProcedureCodes.GetStringProcCode(feeCur.CodeNum)
					+", "+Lan.g(this,"Fee")+": "+feeCur.Amount.ToString("c")+", "+Lan.g(this,"Fee Schedule")+" "+FeeScheds.GetDescription(feeCur.FeeSched)
					+". "+Lan.g(this,"Automatic change to allowed fee in Enter Payment window.  Confirmed by user."),feeCur.CodeNum,DateTime.MinValue);
				SecurityLogs.MakeLogEntry(Permissions.LogFeeEdit,0,Lan.g(this,"Fee Updated"),feeCur.FeeNum,datePrevious);
				invalidFeeSchedNums.Add(feeCur.FeeSched);
			}
		}

		private void butViewEobDetails_Click(object sender,EventArgs e) {
			FormEtrans835ClaimEdit FormE=new FormEtrans835ClaimEdit(_x835,_claimPaid);
			FormE.Show(this);//This window is just used to display information.
		}
		
		///<summary>Called when entering split claim payment information.
		///Returns true if the entered payment rows sum up to the sub set proc information present on this split claim.</summary>
		private bool HasValidSplitClaimTotals() {
			double claimFee=0;
			double dedApplied=0;
			double insPayAmtAllowed=0;
			double insPayAmt=0;
			for(int i=0;i<gridPayments.ListGridRows.Count;i++){
				ClaimProc claimProc=(ClaimProc)gridPayments.ListGridRows[i].Tag;
				if((_isSupplementalPay && claimProc.Status!=ClaimProcStatus.Supplemental)
					|| (!_isSupplementalPay && claimProc.Status!=ClaimProcStatus.Received )
					|| !claimProc.IsNew)
				{
					//Split claims show all the of the original claims procs.
					//But we only enter payment for the procs that are on this split claim.
					continue;
				}
				claimFee+=claimProc.FeeBilled;
				dedApplied+=PIn.Double(gridPayments.ListGridRows[i].Cells[deductIndex].Text);
				insPayAmtAllowed+=PIn.Double(gridPayments.ListGridRows[i].Cells[allowedIndex].Text);
				insPayAmt+=PIn.Double(gridPayments.ListGridRows[i].Cells[insPayEstIndex].Text);
			}
			if(textEobClaimFee.Text!=claimFee.ToString("F")
				|| textEobDedApplied.Text!=dedApplied.ToString("F")
				|| textEobInsPayAllowed.Text!=insPayAmtAllowed.ToString("F")
				|| textEobInsPayAmt.Text!=insPayAmt.ToString("F"))
			{ 
				return false;
			}
			return true;
		}

		private void checkIncludeWOPercCoPay_CheckedChanged(object sender,EventArgs e) {
			if(checkIncludeWOPercCoPay.Checked) {
				//WO's might have been cleared from grid and saved as 0 in claimProc.WriteOff. See SaveGridChanges().
				//Need to set claimProc.WriteOff back to original value when including writeoffs.
				foreach(GridRow row in gridPayments.ListGridRows) {
					ClaimProc claimProcCur=(ClaimProc)row.Tag;
					ClaimProc claimProcOld=_listClaimProcsOld.First(x => x.ClaimProcNum==claimProcCur.ClaimProcNum);
					if(claimProcCur.WriteOff==claimProcOld.WriteOff) {
							continue;
					}
					claimProcCur.WriteOff=claimProcOld.WriteOff;
				}
			}
			FillGridProcedures();
		}
		
		private void butSplitProcs_Click(object sender,EventArgs e) {
			List<GridRow> listSelectedPayRows=gridPayments.ListGridRows.Where(x => x.Cells[splitIndex].Text=="X").ToList();//Split column is "checked".
			if(listSelectedPayRows.Count()==0) {
				MsgBox.Show(this,"Please use the Split column to select at least one procedure.");
				return;
			}
			try{
				SaveGridChanges();//User must zero out the inspay column to allow them to split.  Save changes to claimProcs.
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			#region Claim Validation
			List<ClaimProc> listSelectedClaimProcs=listSelectedPayRows.Select(x => x.Tag as ClaimProc).ToList();
			switch(ClaimL.ClaimIsValid(_claim,listSelectedClaimProcs)) {
				case ClaimIsValidState.False:
				case ClaimIsValidState.FalseClaimProcsChanged://Status of claimProcs were not preauth and claim was preauth.
					return;
				case ClaimIsValidState.True:
					//Good to go.
				break;
			}
			#endregion
			#region Selection validation
			//The following logic mimics FormClaimEdit.butSplit_Click(...)
			if(listSelectedClaimProcs.Any(x => x.ProcNum==0)){
				MsgBox.Show(this,"Only procedures can be selected.  No total payments allowed.  Deselect all total payments before continuing.");
				return;
			}
			if(listSelectedClaimProcs.Any(x => x.InsPayAmt!=0)) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"All selected procedures must have zero insurance payment amounts.\r\nSet all selected insurance payment amounts to zero?")) {
					return;
				}
				listSelectedClaimProcs.ForEach(x => x.InsPayAmt=0);
			}
			//Make sure that there is at least one procedure left on the claim before splitting.
			//The claim would become orphaned if we allow users to split off all procedures on the claim and DBM would be required to run to clean up.
			if(gridPayments.ListGridRows.Count==listSelectedClaimProcs.Count) {//All procedures are selected for the split...
				MsgBox.Show(this,"All procedures were selected.  At least one procedure must remain on this claim after splitting.  Deselect at least one procedure before continuing.");
				return;
			}
			#endregion
			//Point of no return-------------------------------------------------------------------------
			//Selection validation logic ensures these are only procs that are eligible to split from this claim.
			//InsertSplitClaim() changes the database immediately.  If the user cancels the current window, changes will be saved anyway.
			Claim splitClaim=Claims.InsertSplitClaim(_claim,listSelectedClaimProcs);
			#region Split Etrans835Attach
			//Insert a Etrans835Attach row to allow us to associate the split claim back to the original ERA/Claim it is split from.
			//The split attach is inserted immediately.  If the user cancels the current window, changes will be saved anyway.
			Etrans835Attach splitAttach=new Etrans835Attach();
			splitAttach.EtransNum=_x835.EtransSource.EtransNum;
			splitAttach.ClpSegmentIndex=_claimPaid.ClpSegmentIndex;
			splitAttach.ClaimNum=splitClaim.ClaimNum;
			Etrans835Attaches.Insert(splitAttach);
			#endregion
			//Remove all split claimProcs from the internal list, will not show in grid after FillGridProcedures();
			ListClaimProcsForClaim.RemoveAll(x => listSelectedClaimProcs.Contains(x));
			FillGridProcedures();
			MsgBox.Show(this,"Done.");
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			try {
				SaveGridChanges();
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			if(!PrefC.GetBool(PrefName.EraAllowTotalPayments) && gridPayments.ListGridRows.Select(x => (x.Tag as ClaimProc)).Any(x => x.ProcNum==0 && x.InsPayAmt>0)){
				MsgBox.Show("Please allocate all InsPay amounts from Total Payment rows to a procedure before continuing.");
				return;
			}
			bool isPromptNeeded=false;
			if(_claimPaid.IsSplitClaim) {
				isPromptNeeded=!HasValidSplitClaimTotals();//Show prompt if validation fails.
			}
			else if(textEobClaimFee.Text!=textClaimFee.Text
				|| textEobDedApplied.Text!=textDedApplied.Text
				|| textEobInsPayAllowed.Text!=textInsPayAllowed.Text
				|| textEobInsPayAmt.Text!=textInsPayAmt.Text)
			{
				isPromptNeeded=true;
			}
			if(isPromptNeeded && !MsgBox.Show(this,MsgBoxButtons.YesNo,"Some of the EOB totals do not match the totals entered.  Continue?")) {
				return;
			}
			SaveAllowedFees();
			ClaimL.ReceiveEraPayment(_claim,_claimPaid,ListClaimProcsForClaim,checkIncludeWOPercCoPay.Checked,_isSupplementalPay,_insPlan);
			if(PrefC.GetBool(PrefName.ClaimSnapshotEnabled)) {
				Claim claimCur=Claims.GetClaim(_listClaimProcsOld[0].ClaimNum);
				if(claimCur.ClaimType!="PreAuth") {
					ClaimSnapshots.CreateClaimSnapshot(_listClaimProcsOld,ClaimSnapshotTrigger.InsPayment,claimCur.ClaimType);
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
