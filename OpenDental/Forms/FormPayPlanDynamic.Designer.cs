using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDental {
	partial class FormPayPlanDynamic {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
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
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayPlanDynamic));
			this.labelTotalTx = new System.Windows.Forms.Label();
			this.textTotalTxAmt = new OpenDental.ValidDouble();
			this.label1 = new System.Windows.Forms.Label();
			this.textCompletedAmt = new OpenDental.ValidDouble();
			this.textPrincPaid = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.textNote = new OpenDental.ODtextBox();
			this.butDelete = new OpenDental.UI.Button();
			this.textAccumulatedDue = new System.Windows.Forms.TextBox();
			this.textAmtPaid = new System.Windows.Forms.TextBox();
			this.butGoToPat = new OpenDental.UI.Button();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.butGoToGuar = new OpenDental.UI.Button();
			this.textDate = new OpenDental.ValidDate();
			this.butChangeGuar = new OpenDental.UI.Button();
			this.textGuarantor = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupTerms = new System.Windows.Forms.GroupBox();
			this.label16 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.textPaymentCount = new OpenDental.ValidNumber();
			this.textPeriodPayment = new OpenDental.ValidDouble();
			this.groupBoxFrequency = new System.Windows.Forms.GroupBox();
			this.radioQuarterly = new System.Windows.Forms.RadioButton();
			this.radioMonthly = new System.Windows.Forms.RadioButton();
			this.radioOrdinalWeekday = new System.Windows.Forms.RadioButton();
			this.radioEveryOtherWeek = new System.Windows.Forms.RadioButton();
			this.radioWeekly = new System.Windows.Forms.RadioButton();
			this.butSave = new OpenDental.UI.Button();
			this.butCancelTerms = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.textAPR = new OpenDental.ValidDouble();
			this.textDownPayment = new OpenDental.ValidDouble();
			this.label11 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textDateFirstPay = new OpenDental.ValidDate();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textTotalPrincipal = new OpenDental.ValidDouble();
			this.butCreateSched = new OpenDental.UI.Button();
			this.labelDateAgreement = new System.Windows.Forms.Label();
			this.butUnlock = new OpenDental.UI.Button();
			this.labelGuarantor = new System.Windows.Forms.Label();
			this.textTotalCost = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.butPrint = new OpenDental.UI.Button();
			this.butClosePlan = new OpenDental.UI.Button();
			this.labelClosed = new System.Windows.Forms.Label();
			this.butSignPrint = new OpenDental.UI.Button();
			this.signatureBoxWrapper = new OpenDental.UI.SignatureBoxWrapper();
			this.groupBoxSignature = new System.Windows.Forms.GroupBox();
			this.comboCategory = new OpenDental.UI.ComboBoxPlus();
			this.label2 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabSchedule = new System.Windows.Forms.TabPage();
			this.textBalanceSum = new OpenDental.ValidDouble();
			this.checkExcludePast = new System.Windows.Forms.CheckBox();
			this.textDueSum = new System.Windows.Forms.TextBox();
			this.textInterestSum = new System.Windows.Forms.TextBox();
			this.textPaymentSum = new OpenDental.ValidDouble();
			this.textPrincipalSum = new OpenDental.ValidDouble();
			this.labelTotals = new System.Windows.Forms.Label();
			this.gridCharges = new OpenDental.UI.ODGrid();
			this.tabProduction = new System.Windows.Forms.TabPage();
			this.butDeleteProduction = new OpenDental.UI.Button();
			this.butAddAdjustment = new OpenDental.UI.Button();
			this.butPrintProduction = new OpenDental.UI.Button();
			this.butAddProcedure = new OpenDental.UI.Button();
			this.gridLinkedProduction = new OpenDental.UI.ODGrid();
			this.contextMenuLinkedProduction = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.checkProductionLock = new System.Windows.Forms.CheckBox();
			this.labelOverpaidWarning = new System.Windows.Forms.Label();
			this.groupTerms.SuspendLayout();
			this.groupBoxFrequency.SuspendLayout();
			this.groupBoxSignature.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabSchedule.SuspendLayout();
			this.tabProduction.SuspendLayout();
			this.contextMenuLinkedProduction.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelTotalTx
			// 
			this.labelTotalTx.Location = new System.Drawing.Point(6, 528);
			this.labelTotalTx.Name = "labelTotalTx";
			this.labelTotalTx.Size = new System.Drawing.Size(141, 17);
			this.labelTotalTx.TabIndex = 147;
			this.labelTotalTx.Text = "Total Tx Amount";
			this.labelTotalTx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotalTxAmt
			// 
			this.textTotalTxAmt.Location = new System.Drawing.Point(148, 526);
			this.textTotalTxAmt.MaxVal = 100000000D;
			this.textTotalTxAmt.MinVal = -100000000D;
			this.textTotalTxAmt.Name = "textTotalTxAmt";
			this.textTotalTxAmt.ReadOnly = true;
			this.textTotalTxAmt.Size = new System.Drawing.Size(85, 20);
			this.textTotalTxAmt.TabIndex = 148;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 506);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(141, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tx Completed Amount";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCompletedAmt
			// 
			this.textCompletedAmt.Location = new System.Drawing.Point(148, 504);
			this.textCompletedAmt.MaxVal = 100000000D;
			this.textCompletedAmt.MinVal = -100000000D;
			this.textCompletedAmt.Name = "textCompletedAmt";
			this.textCompletedAmt.ReadOnly = true;
			this.textCompletedAmt.Size = new System.Drawing.Size(85, 20);
			this.textCompletedAmt.TabIndex = 2;
			// 
			// textPrincPaid
			// 
			this.textPrincPaid.Location = new System.Drawing.Point(148, 482);
			this.textPrincPaid.Name = "textPrincPaid";
			this.textPrincPaid.ReadOnly = true;
			this.textPrincPaid.Size = new System.Drawing.Size(85, 20);
			this.textPrincPaid.TabIndex = 0;
			this.textPrincPaid.TabStop = false;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(6, 484);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(141, 17);
			this.label14.TabIndex = 0;
			this.label14.Text = "Principal paid so far";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(373, 561);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.PayPlan;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(662, 94);
			this.textNote.SpellCheckIsEnabled = false;
			this.textNote.TabIndex = 3;
			this.textNote.TabStop = false;
			this.textNote.Text = "";
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(12, 674);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(84, 24);
			this.butDelete.TabIndex = 9;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textAccumulatedDue
			// 
			this.textAccumulatedDue.Location = new System.Drawing.Point(148, 438);
			this.textAccumulatedDue.Name = "textAccumulatedDue";
			this.textAccumulatedDue.ReadOnly = true;
			this.textAccumulatedDue.Size = new System.Drawing.Size(85, 20);
			this.textAccumulatedDue.TabIndex = 0;
			this.textAccumulatedDue.TabStop = false;
			// 
			// textAmtPaid
			// 
			this.textAmtPaid.Location = new System.Drawing.Point(148, 460);
			this.textAmtPaid.Name = "textAmtPaid";
			this.textAmtPaid.ReadOnly = true;
			this.textAmtPaid.Size = new System.Drawing.Size(85, 20);
			this.textAmtPaid.TabIndex = 0;
			this.textAmtPaid.TabStop = false;
			// 
			// butGoToPat
			// 
			this.butGoToPat.Location = new System.Drawing.Point(252, 26);
			this.butGoToPat.Name = "butGoToPat";
			this.butGoToPat.Size = new System.Drawing.Size(44, 22);
			this.butGoToPat.TabIndex = 10;
			this.butGoToPat.Text = "&Go To";
			this.butGoToPat.Click += new System.EventHandler(this.butGoToPat_Click);
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(74, 27);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(177, 20);
			this.textPatient.TabIndex = 0;
			this.textPatient.TabStop = false;
			// 
			// butGoToGuar
			// 
			this.butGoToGuar.Location = new System.Drawing.Point(252, 48);
			this.butGoToGuar.Name = "butGoToGuar";
			this.butGoToGuar.Size = new System.Drawing.Size(44, 22);
			this.butGoToGuar.TabIndex = 11;
			this.butGoToGuar.Text = "Go &To";
			this.butGoToGuar.Click += new System.EventHandler(this.butGoTo_Click);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(142, 14);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(85, 20);
			this.textDate.TabIndex = 16;
			// 
			// butChangeGuar
			// 
			this.butChangeGuar.Location = new System.Drawing.Point(298, 48);
			this.butChangeGuar.Name = "butChangeGuar";
			this.butChangeGuar.Size = new System.Drawing.Size(65, 22);
			this.butChangeGuar.TabIndex = 12;
			this.butChangeGuar.Text = "C&hange";
			this.butChangeGuar.Click += new System.EventHandler(this.butChangeGuar_Click);
			// 
			// textGuarantor
			// 
			this.textGuarantor.Location = new System.Drawing.Point(74, 49);
			this.textGuarantor.Name = "textGuarantor";
			this.textGuarantor.ReadOnly = true;
			this.textGuarantor.Size = new System.Drawing.Size(177, 20);
			this.textGuarantor.TabIndex = 0;
			this.textGuarantor.TabStop = false;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(881, 674);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 7;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(959, 674);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(375, 543);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(92, 17);
			this.label10.TabIndex = 0;
			this.label10.Text = "Note";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(6, 440);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(141, 17);
			this.label13.TabIndex = 0;
			this.label13.Text = "Accumulated Due";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 462);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(141, 17);
			this.label12.TabIndex = 0;
			this.label12.Text = "Paid so far";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(1, 27);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(71, 20);
			this.label9.TabIndex = 0;
			this.label9.Text = "Patient";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupTerms
			// 
			this.groupTerms.Controls.Add(this.label16);
			this.groupTerms.Controls.Add(this.label7);
			this.groupTerms.Controls.Add(this.textPaymentCount);
			this.groupTerms.Controls.Add(this.textPeriodPayment);
			this.groupTerms.Controls.Add(this.groupBoxFrequency);
			this.groupTerms.Controls.Add(this.butSave);
			this.groupTerms.Controls.Add(this.butCancelTerms);
			this.groupTerms.Controls.Add(this.label8);
			this.groupTerms.Controls.Add(this.textAPR);
			this.groupTerms.Controls.Add(this.textDownPayment);
			this.groupTerms.Controls.Add(this.label11);
			this.groupTerms.Controls.Add(this.label6);
			this.groupTerms.Controls.Add(this.textDateFirstPay);
			this.groupTerms.Controls.Add(this.label5);
			this.groupTerms.Controls.Add(this.label4);
			this.groupTerms.Controls.Add(this.textTotalPrincipal);
			this.groupTerms.Controls.Add(this.butCreateSched);
			this.groupTerms.Controls.Add(this.textDate);
			this.groupTerms.Controls.Add(this.labelDateAgreement);
			this.groupTerms.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupTerms.Location = new System.Drawing.Point(4, 102);
			this.groupTerms.Name = "groupTerms";
			this.groupTerms.Size = new System.Drawing.Size(360, 298);
			this.groupTerms.TabIndex = 1;
			this.groupTerms.TabStop = false;
			this.groupTerms.Text = "Terms";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(190, 147);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(124, 17);
			this.label16.TabIndex = 195;
			this.label16.Text = "(sets payment amount)";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(17, 124);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(122, 17);
			this.label7.TabIndex = 0;
			this.label7.Text = "Payment Amount";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPaymentCount
			// 
			this.textPaymentCount.Location = new System.Drawing.Point(142, 145);
			this.textPaymentCount.MaxVal = 255;
			this.textPaymentCount.MinVal = 1;
			this.textPaymentCount.Name = "textPaymentCount";
			this.textPaymentCount.Size = new System.Drawing.Size(47, 20);
			this.textPaymentCount.TabIndex = 1;
			this.textPaymentCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPaymentCount_KeyPress);
			// 
			// textPeriodPayment
			// 
			this.textPeriodPayment.Location = new System.Drawing.Point(142, 123);
			this.textPeriodPayment.MaxVal = 100000000D;
			this.textPeriodPayment.MinVal = 0.01D;
			this.textPeriodPayment.Name = "textPeriodPayment";
			this.textPeriodPayment.Size = new System.Drawing.Size(85, 20);
			this.textPeriodPayment.TabIndex = 2;
			this.textPeriodPayment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPeriodPayment_KeyPress);
			// 
			// groupBoxFrequency
			// 
			this.groupBoxFrequency.Controls.Add(this.radioQuarterly);
			this.groupBoxFrequency.Controls.Add(this.radioMonthly);
			this.groupBoxFrequency.Controls.Add(this.radioOrdinalWeekday);
			this.groupBoxFrequency.Controls.Add(this.radioEveryOtherWeek);
			this.groupBoxFrequency.Controls.Add(this.radioWeekly);
			this.groupBoxFrequency.Location = new System.Drawing.Point(8, 168);
			this.groupBoxFrequency.Name = "groupBoxFrequency";
			this.groupBoxFrequency.Size = new System.Drawing.Size(346, 123);
			this.groupBoxFrequency.TabIndex = 194;
			this.groupBoxFrequency.TabStop = false;
			this.groupBoxFrequency.Text = "Charge Frequency";
			// 
			// radioQuarterly
			// 
			this.radioQuarterly.Location = new System.Drawing.Point(5, 95);
			this.radioQuarterly.Name = "radioQuarterly";
			this.radioQuarterly.Size = new System.Drawing.Size(104, 17);
			this.radioQuarterly.TabIndex = 28;
			this.radioQuarterly.TabStop = true;
			this.radioQuarterly.Text = "Quarterly";
			this.radioQuarterly.UseVisualStyleBackColor = true;
			// 
			// radioMonthly
			// 
			this.radioMonthly.Checked = true;
			this.radioMonthly.Location = new System.Drawing.Point(5, 75);
			this.radioMonthly.Name = "radioMonthly";
			this.radioMonthly.Size = new System.Drawing.Size(104, 17);
			this.radioMonthly.TabIndex = 27;
			this.radioMonthly.TabStop = true;
			this.radioMonthly.Text = "Monthly";
			this.radioMonthly.UseVisualStyleBackColor = true;
			// 
			// radioOrdinalWeekday
			// 
			this.radioOrdinalWeekday.Location = new System.Drawing.Point(5, 55);
			this.radioOrdinalWeekday.Name = "radioOrdinalWeekday";
			this.radioOrdinalWeekday.Size = new System.Drawing.Size(264, 17);
			this.radioOrdinalWeekday.TabIndex = 26;
			this.radioOrdinalWeekday.TabStop = true;
			this.radioOrdinalWeekday.Text = "Specific day of month";
			this.radioOrdinalWeekday.UseVisualStyleBackColor = true;
			// 
			// radioEveryOtherWeek
			// 
			this.radioEveryOtherWeek.Location = new System.Drawing.Point(5, 35);
			this.radioEveryOtherWeek.Name = "radioEveryOtherWeek";
			this.radioEveryOtherWeek.Size = new System.Drawing.Size(156, 17);
			this.radioEveryOtherWeek.TabIndex = 25;
			this.radioEveryOtherWeek.TabStop = true;
			this.radioEveryOtherWeek.Text = "Every other week";
			this.radioEveryOtherWeek.UseVisualStyleBackColor = true;
			// 
			// radioWeekly
			// 
			this.radioWeekly.Location = new System.Drawing.Point(6, 15);
			this.radioWeekly.Name = "radioWeekly";
			this.radioWeekly.Size = new System.Drawing.Size(104, 17);
			this.radioWeekly.TabIndex = 24;
			this.radioWeekly.TabStop = true;
			this.radioWeekly.Text = "Weekly";
			this.radioWeekly.UseVisualStyleBackColor = true;
			// 
			// butSave
			// 
			this.butSave.Location = new System.Drawing.Point(310, 36);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(44, 22);
			this.butSave.TabIndex = 193;
			this.butSave.Text = "Save";
			this.butSave.Click += new System.EventHandler(this.ButSave_Click);
			// 
			// butCancelTerms
			// 
			this.butCancelTerms.Location = new System.Drawing.Point(251, 36);
			this.butCancelTerms.Name = "butCancelTerms";
			this.butCancelTerms.Size = new System.Drawing.Size(57, 22);
			this.butCancelTerms.TabIndex = 192;
			this.butCancelTerms.Text = "Cancel";
			this.butCancelTerms.Click += new System.EventHandler(this.ButCancelTerms_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(19, 146);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(124, 17);
			this.label8.TabIndex = 0;
			this.label8.Text = "Number of Payments";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAPR
			// 
			this.textAPR.Location = new System.Drawing.Point(142, 101);
			this.textAPR.MaxVal = 100000000D;
			this.textAPR.MinVal = 0D;
			this.textAPR.Name = "textAPR";
			this.textAPR.Size = new System.Drawing.Size(47, 20);
			this.textAPR.TabIndex = 4;
			// 
			// textDownPayment
			// 
			this.textDownPayment.Location = new System.Drawing.Point(142, 79);
			this.textDownPayment.MaxVal = 100000000D;
			this.textDownPayment.MinVal = 0D;
			this.textDownPayment.Name = "textDownPayment";
			this.textDownPayment.Size = new System.Drawing.Size(85, 20);
			this.textDownPayment.TabIndex = 3;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(4, 82);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(136, 17);
			this.label11.TabIndex = 0;
			this.label11.Text = "Down Payment";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(3, 103);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(138, 17);
			this.label6.TabIndex = 0;
			this.label6.Text = "APR (for example 0 or 18)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateFirstPay
			// 
			this.textDateFirstPay.Location = new System.Drawing.Point(142, 57);
			this.textDateFirstPay.Name = "textDateFirstPay";
			this.textDateFirstPay.Size = new System.Drawing.Size(85, 20);
			this.textDateFirstPay.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(5, 59);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(135, 17);
			this.label5.TabIndex = 0;
			this.label5.Text = "Date of First Payment";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5, 37);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(134, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "Total Principal Amount";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotalPrincipal
			// 
			this.textTotalPrincipal.Location = new System.Drawing.Point(142, 36);
			this.textTotalPrincipal.MaxVal = 100000000D;
			this.textTotalPrincipal.MinVal = 0.01D;
			this.textTotalPrincipal.Name = "textTotalPrincipal";
			this.textTotalPrincipal.ReadOnly = true;
			this.textTotalPrincipal.Size = new System.Drawing.Size(85, 20);
			this.textTotalPrincipal.TabIndex = 1;
			this.textTotalPrincipal.Validating += new System.ComponentModel.CancelEventHandler(this.textAmount_Validating);
			// 
			// butCreateSched
			// 
			this.butCreateSched.Location = new System.Drawing.Point(251, 12);
			this.butCreateSched.Name = "butCreateSched";
			this.butCreateSched.Size = new System.Drawing.Size(103, 22);
			this.butCreateSched.TabIndex = 6;
			this.butCreateSched.Text = "Create Schedule";
			this.butCreateSched.Click += new System.EventHandler(this.butCreateSched_Click);
			// 
			// labelDateAgreement
			// 
			this.labelDateAgreement.Location = new System.Drawing.Point(23, 15);
			this.labelDateAgreement.Name = "labelDateAgreement";
			this.labelDateAgreement.Size = new System.Drawing.Size(117, 17);
			this.labelDateAgreement.TabIndex = 0;
			this.labelDateAgreement.Text = "Date of Agreement";
			this.labelDateAgreement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butUnlock
			// 
			this.butUnlock.Location = new System.Drawing.Point(307, 84);
			this.butUnlock.Name = "butUnlock";
			this.butUnlock.Size = new System.Drawing.Size(57, 22);
			this.butUnlock.TabIndex = 191;
			this.butUnlock.Text = "Unlock";
			this.butUnlock.Click += new System.EventHandler(this.ButUnlock_Click);
			// 
			// labelGuarantor
			// 
			this.labelGuarantor.Location = new System.Drawing.Point(1, 49);
			this.labelGuarantor.Name = "labelGuarantor";
			this.labelGuarantor.Size = new System.Drawing.Size(75, 20);
			this.labelGuarantor.TabIndex = 0;
			this.labelGuarantor.Text = "Guarantor";
			this.labelGuarantor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotalCost
			// 
			this.textTotalCost.Location = new System.Drawing.Point(148, 416);
			this.textTotalCost.Name = "textTotalCost";
			this.textTotalCost.ReadOnly = true;
			this.textTotalCost.Size = new System.Drawing.Size(85, 20);
			this.textTotalCost.TabIndex = 0;
			this.textTotalCost.TabStop = false;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(6, 416);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(139, 17);
			this.label15.TabIndex = 0;
			this.label15.Text = "Total Cost of Loan";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPrint
			// 
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(526, 674);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(85, 24);
			this.butPrint.TabIndex = 6;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butClosePlan
			// 
			this.butClosePlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butClosePlan.Image = global::OpenDental.Properties.Resources.close_door;
			this.butClosePlan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butClosePlan.Location = new System.Drawing.Point(102, 674);
			this.butClosePlan.Name = "butClosePlan";
			this.butClosePlan.Size = new System.Drawing.Size(84, 24);
			this.butClosePlan.TabIndex = 149;
			this.butClosePlan.Text = "Close Plan";
			this.butClosePlan.Click += new System.EventHandler(this.butCloseOut_Click);
			// 
			// labelClosed
			// 
			this.labelClosed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelClosed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelClosed.ForeColor = System.Drawing.Color.Red;
			this.labelClosed.Location = new System.Drawing.Point(526, 657);
			this.labelClosed.Name = "labelClosed";
			this.labelClosed.Size = new System.Drawing.Size(512, 15);
			this.labelClosed.TabIndex = 150;
			this.labelClosed.Text = "This payment plan is closed. You must click \"Reopen\" before editing it";
			this.labelClosed.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelClosed.Visible = false;
			// 
			// butSignPrint
			// 
			this.butSignPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSignPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSignPrint.Location = new System.Drawing.Point(431, 674);
			this.butSignPrint.Name = "butSignPrint";
			this.butSignPrint.Size = new System.Drawing.Size(92, 24);
			this.butSignPrint.TabIndex = 151;
			this.butSignPrint.Text = "Sign && Print";
			this.butSignPrint.Visible = false;
			this.butSignPrint.Click += new System.EventHandler(this.butSignPrint_Click);
			// 
			// signatureBoxWrapper
			// 
			this.signatureBoxWrapper.BackColor = System.Drawing.SystemColors.ControlDark;
			this.signatureBoxWrapper.Enabled = false;
			this.signatureBoxWrapper.Location = new System.Drawing.Point(6, 19);
			this.signatureBoxWrapper.Name = "signatureBoxWrapper";
			this.signatureBoxWrapper.SignatureMode = OpenDental.UI.SignatureBoxWrapper.SigMode.Default;
			this.signatureBoxWrapper.Size = new System.Drawing.Size(351, 65);
			this.signatureBoxWrapper.TabIndex = 183;
			this.signatureBoxWrapper.UserSig = null;
			this.signatureBoxWrapper.Visible = false;
			// 
			// groupBoxSignature
			// 
			this.groupBoxSignature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxSignature.Controls.Add(this.signatureBoxWrapper);
			this.groupBoxSignature.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBoxSignature.Location = new System.Drawing.Point(7, 560);
			this.groupBoxSignature.Name = "groupBoxSignature";
			this.groupBoxSignature.Size = new System.Drawing.Size(363, 96);
			this.groupBoxSignature.TabIndex = 184;
			this.groupBoxSignature.TabStop = false;
			this.groupBoxSignature.Text = "Signature";
			this.groupBoxSignature.Visible = false;
			// 
			// comboCategory
			// 
			this.comboCategory.Location = new System.Drawing.Point(74, 4);
			this.comboCategory.Name = "comboCategory";
			this.comboCategory.Size = new System.Drawing.Size(177, 21);
			this.comboCategory.TabIndex = 187;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(1, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 18);
			this.label2.TabIndex = 188;
			this.label2.Text = "Category";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabSchedule);
			this.tabControl1.Controls.Add(this.tabProduction);
			this.tabControl1.Location = new System.Drawing.Point(369, 21);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(673, 524);
			this.tabControl1.TabIndex = 192;
			// 
			// tabSchedule
			// 
			this.tabSchedule.BackColor = System.Drawing.SystemColors.Control;
			this.tabSchedule.Controls.Add(this.textBalanceSum);
			this.tabSchedule.Controls.Add(this.checkExcludePast);
			this.tabSchedule.Controls.Add(this.textDueSum);
			this.tabSchedule.Controls.Add(this.textInterestSum);
			this.tabSchedule.Controls.Add(this.textPaymentSum);
			this.tabSchedule.Controls.Add(this.textPrincipalSum);
			this.tabSchedule.Controls.Add(this.labelTotals);
			this.tabSchedule.Controls.Add(this.gridCharges);
			this.tabSchedule.Location = new System.Drawing.Point(4, 22);
			this.tabSchedule.Name = "tabSchedule";
			this.tabSchedule.Padding = new System.Windows.Forms.Padding(3);
			this.tabSchedule.Size = new System.Drawing.Size(665, 498);
			this.tabSchedule.TabIndex = 0;
			this.tabSchedule.Tag = "gridCharges[OpenDental.UI.ODGrid]";
			this.tabSchedule.Text = "Schedule";
			// 
			// textBalanceSum
			// 
			this.textBalanceSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBalanceSum.Location = new System.Drawing.Point(557, 475);
			this.textBalanceSum.MaxVal = 100000000D;
			this.textBalanceSum.MinVal = -100000000D;
			this.textBalanceSum.Name = "textBalanceSum";
			this.textBalanceSum.ReadOnly = true;
			this.textBalanceSum.Size = new System.Drawing.Size(87, 20);
			this.textBalanceSum.TabIndex = 199;
			this.textBalanceSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkExcludePast
			// 
			this.checkExcludePast.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludePast.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludePast.Location = new System.Drawing.Point(2, 478);
			this.checkExcludePast.Name = "checkExcludePast";
			this.checkExcludePast.Size = new System.Drawing.Size(144, 17);
			this.checkExcludePast.TabIndex = 198;
			this.checkExcludePast.Text = "Exclude past activity";
			this.checkExcludePast.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.checkExcludePast.UseVisualStyleBackColor = true;
			this.checkExcludePast.CheckedChanged += new System.EventHandler(this.checkExcludePast_CheckedChanged);
			// 
			// textDueSum
			// 
			this.textDueSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDueSum.Location = new System.Drawing.Point(407, 475);
			this.textDueSum.Name = "textDueSum";
			this.textDueSum.ReadOnly = true;
			this.textDueSum.Size = new System.Drawing.Size(76, 20);
			this.textDueSum.TabIndex = 197;
			this.textDueSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textInterestSum
			// 
			this.textInterestSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textInterestSum.Location = new System.Drawing.Point(340, 475);
			this.textInterestSum.Name = "textInterestSum";
			this.textInterestSum.ReadOnly = true;
			this.textInterestSum.Size = new System.Drawing.Size(68, 20);
			this.textInterestSum.TabIndex = 194;
			this.textInterestSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPaymentSum
			// 
			this.textPaymentSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textPaymentSum.Location = new System.Drawing.Point(482, 475);
			this.textPaymentSum.MaxVal = 100000000D;
			this.textPaymentSum.MinVal = -100000000D;
			this.textPaymentSum.Name = "textPaymentSum";
			this.textPaymentSum.ReadOnly = true;
			this.textPaymentSum.Size = new System.Drawing.Size(76, 20);
			this.textPaymentSum.TabIndex = 193;
			this.textPaymentSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPrincipalSum
			// 
			this.textPrincipalSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textPrincipalSum.Location = new System.Drawing.Point(265, 475);
			this.textPrincipalSum.MaxVal = 100000000D;
			this.textPrincipalSum.MinVal = -100000000D;
			this.textPrincipalSum.Name = "textPrincipalSum";
			this.textPrincipalSum.ReadOnly = true;
			this.textPrincipalSum.Size = new System.Drawing.Size(76, 20);
			this.textPrincipalSum.TabIndex = 192;
			this.textPrincipalSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelTotals
			// 
			this.labelTotals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTotals.Location = new System.Drawing.Point(154, 477);
			this.labelTotals.Name = "labelTotals";
			this.labelTotals.Size = new System.Drawing.Size(104, 18);
			this.labelTotals.TabIndex = 195;
			this.labelTotals.Text = "Current Totals";
			this.labelTotals.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// gridCharges
			// 
			this.gridCharges.Dock = System.Windows.Forms.DockStyle.Top;
			this.gridCharges.Location = new System.Drawing.Point(3, 3);
			this.gridCharges.Name = "gridCharges";
			this.gridCharges.Size = new System.Drawing.Size(659, 473);
			this.gridCharges.TabIndex = 191;
			this.gridCharges.Tag = "";
			this.gridCharges.Title = "Amortization Schedule";
			this.gridCharges.TranslationName = "PayPlanAmortization";
			this.gridCharges.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCharges_CellDoubleClick);
			// 
			// tabProduction
			// 
			this.tabProduction.BackColor = System.Drawing.SystemColors.Control;
			this.tabProduction.Controls.Add(this.butDeleteProduction);
			this.tabProduction.Controls.Add(this.butAddAdjustment);
			this.tabProduction.Controls.Add(this.butPrintProduction);
			this.tabProduction.Controls.Add(this.butAddProcedure);
			this.tabProduction.Controls.Add(this.gridLinkedProduction);
			this.tabProduction.Location = new System.Drawing.Point(4, 22);
			this.tabProduction.Name = "tabProduction";
			this.tabProduction.Padding = new System.Windows.Forms.Padding(3);
			this.tabProduction.Size = new System.Drawing.Size(665, 498);
			this.tabProduction.TabIndex = 1;
			this.tabProduction.Tag = "gridLinkedProduction[OpenDental.UI.ODGrid]";
			this.tabProduction.Text = "Production";
			// 
			// butDeleteProduction
			// 
			this.butDeleteProduction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDeleteProduction.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDeleteProduction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDeleteProduction.Location = new System.Drawing.Point(3, 473);
			this.butDeleteProduction.Name = "butDeleteProduction";
			this.butDeleteProduction.Size = new System.Drawing.Size(84, 22);
			this.butDeleteProduction.TabIndex = 35;
			this.butDeleteProduction.Text = "&Delete";
			this.butDeleteProduction.Click += new System.EventHandler(this.ButDeleteProduction_Click);
			// 
			// butAddAdjustment
			// 
			this.butAddAdjustment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddAdjustment.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddAdjustment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddAdjustment.Location = new System.Drawing.Point(482, 473);
			this.butAddAdjustment.Name = "butAddAdjustment";
			this.butAddAdjustment.Size = new System.Drawing.Size(95, 22);
			this.butAddAdjustment.TabIndex = 34;
			this.butAddAdjustment.Text = "Adjustment";
			this.butAddAdjustment.UseVisualStyleBackColor = true;
			this.butAddAdjustment.Click += new System.EventHandler(this.ButAddAdjustment_Click);
			// 
			// butPrintProduction
			// 
			this.butPrintProduction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrintProduction.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrintProduction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrintProduction.Location = new System.Drawing.Point(296, 473);
			this.butPrintProduction.Name = "butPrintProduction";
			this.butPrintProduction.Size = new System.Drawing.Size(73, 22);
			this.butPrintProduction.TabIndex = 31;
			this.butPrintProduction.Text = "Print";
			this.butPrintProduction.Click += new System.EventHandler(this.ButPrintProduction_Click);
			// 
			// butAddProcedure
			// 
			this.butAddProcedure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddProcedure.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddProcedure.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddProcedure.Location = new System.Drawing.Point(579, 473);
			this.butAddProcedure.Name = "butAddProcedure";
			this.butAddProcedure.Size = new System.Drawing.Size(83, 22);
			this.butAddProcedure.TabIndex = 33;
			this.butAddProcedure.Text = "Procedure";
			this.butAddProcedure.UseVisualStyleBackColor = true;
			this.butAddProcedure.Click += new System.EventHandler(this.ButAddProcedure_Click);
			// 
			// gridLinkedProduction
			// 
			this.gridLinkedProduction.HasMultilineHeaders = true;
			this.gridLinkedProduction.Location = new System.Drawing.Point(3, 3);
			this.gridLinkedProduction.Name = "gridLinkedProduction";
			this.gridLinkedProduction.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridLinkedProduction.Size = new System.Drawing.Size(659, 465);
			this.gridLinkedProduction.TabIndex = 0;
			this.gridLinkedProduction.Title = "Attached Production  ";
			this.gridLinkedProduction.TranslationName = "TablePaymentPlanCredits";
			this.gridLinkedProduction.CellLeave += new OpenDental.UI.ODGridClickEventHandler(this.gridLinkedProduction_CellLeave);
			// 
			// contextMenuLinkedProduction
			// 
			this.contextMenuLinkedProduction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
			this.contextMenuLinkedProduction.Name = "contextMenuLinkedProduction";
			this.contextMenuLinkedProduction.Size = new System.Drawing.Size(118, 26);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
			this.toolStripMenuItem1.Text = "Remove";
			// 
			// checkProductionLock
			// 
			this.checkProductionLock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.checkProductionLock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProductionLock.Location = new System.Drawing.Point(163, 88);
			this.checkProductionLock.Name = "checkProductionLock";
			this.checkProductionLock.Size = new System.Drawing.Size(133, 18);
			this.checkProductionLock.TabIndex = 199;
			this.checkProductionLock.Text = "Full Lock";
			this.checkProductionLock.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.checkProductionLock.UseVisualStyleBackColor = true;
			this.checkProductionLock.CheckedChanged += new System.EventHandler(this.CheckProductionLock_CheckedChanged);
			// 
			// labelOverpaidWarning
			// 
			this.labelOverpaidWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelOverpaidWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelOverpaidWarning.ForeColor = System.Drawing.Color.Red;
			this.labelOverpaidWarning.Location = new System.Drawing.Point(678, 4);
			this.labelOverpaidWarning.Name = "labelOverpaidWarning";
			this.labelOverpaidWarning.Size = new System.Drawing.Size(357, 14);
			this.labelOverpaidWarning.TabIndex = 201;
			this.labelOverpaidWarning.Text = "This payment plan has overcharged production";
			this.labelOverpaidWarning.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelOverpaidWarning.Visible = false;
			// 
			// FormPayPlanDynamic
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1043, 709);
			this.Controls.Add(this.labelOverpaidWarning);
			this.Controls.Add(this.checkProductionLock);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butUnlock);
			this.Controls.Add(this.comboCategory);
			this.Controls.Add(this.groupBoxSignature);
			this.Controls.Add(this.butSignPrint);
			this.Controls.Add(this.labelClosed);
			this.Controls.Add(this.butClosePlan);
			this.Controls.Add(this.labelTotalTx);
			this.Controls.Add(this.textTotalTxAmt);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textCompletedAmt);
			this.Controls.Add(this.textPrincPaid);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textAccumulatedDue);
			this.Controls.Add(this.textAmtPaid);
			this.Controls.Add(this.butGoToPat);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.butGoToGuar);
			this.Controls.Add(this.butChangeGuar);
			this.Controls.Add(this.textGuarantor);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.groupTerms);
			this.Controls.Add(this.labelGuarantor);
			this.Controls.Add(this.textTotalCost);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.butPrint);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1059, 748);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(1059, 748);
			this.Name = "FormPayPlanDynamic";
			this.ShowInTaskbar = false;
			this.Text = "Dynamic Payment Plan";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormPayPlanDynamic_Closing);
			this.Load += new System.EventHandler(this.FormPayPlanDynamic_Load);
			this.groupTerms.ResumeLayout(false);
			this.groupTerms.PerformLayout();
			this.groupBoxFrequency.ResumeLayout(false);
			this.groupBoxSignature.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabSchedule.ResumeLayout(false);
			this.tabSchedule.PerformLayout();
			this.tabProduction.ResumeLayout(false);
			this.contextMenuLinkedProduction.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label labelDateAgreement;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox groupTerms;
		private OpenDental.ValidDate textDate;
		private OpenDental.ValidDouble textTotalPrincipal;
		private OpenDental.ValidDate textDateFirstPay;
		private OpenDental.ValidDouble textAPR;
		private OpenDental.UI.Button butPrint;
		private System.Windows.Forms.TextBox textGuarantor;
		private OpenDental.UI.Button butGoToGuar;
		private OpenDental.UI.Button butGoToPat;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private OpenDental.ValidDouble textDownPayment;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox textTotalCost;
		private System.Windows.Forms.Label label10;
		private OpenDental.UI.Button butDelete;
		private OpenDental.ODtextBox textNote;
		private System.Windows.Forms.TextBox textAccumulatedDue;
		private OpenDental.UI.Button butCreateSched;
		private OpenDental.ValidDouble textPeriodPayment;
		private OpenDental.UI.Button butChangeGuar;
		private System.Windows.Forms.Label labelGuarantor;
		private System.Windows.Forms.TextBox textAmtPaid;
		private System.Windows.Forms.TextBox textPrincPaid;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label1;
		private ValidDouble textCompletedAmt;
		private System.Windows.Forms.Label labelTotalTx;
		private ValidDouble textTotalTxAmt;
		private UI.Button butClosePlan;
		private System.Windows.Forms.Label labelClosed;
		private UI.Button butSignPrint;
		private UI.SignatureBoxWrapper signatureBoxWrapper;
		private System.Windows.Forms.GroupBox groupBoxSignature;
		private ValidNumber textPaymentCount;
		private UI.ComboBoxPlus comboCategory;
		private System.Windows.Forms.Label label2;
		private UI.Button butUnlock;
		private UI.Button butSave;
		private UI.Button butCancelTerms;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabSchedule;
		private ValidDouble textBalanceSum;
		private System.Windows.Forms.CheckBox checkExcludePast;
		private System.Windows.Forms.TextBox textDueSum;
		private System.Windows.Forms.TextBox textInterestSum;
		private ValidDouble textPaymentSum;
		private ValidDouble textPrincipalSum;
		private System.Windows.Forms.Label labelTotals;
		private UI.ODGrid gridCharges;
		private System.Windows.Forms.TabPage tabProduction;
		private UI.ODGrid gridLinkedProduction;
		private UI.Button butAddProcedure;
		private UI.Button butPrintProduction;
		private System.Windows.Forms.GroupBox groupBoxFrequency;
		public System.Windows.Forms.RadioButton radioWeekly;
		public System.Windows.Forms.RadioButton radioEveryOtherWeek;
		public System.Windows.Forms.RadioButton radioOrdinalWeekday;
		public System.Windows.Forms.RadioButton radioMonthly;
		public System.Windows.Forms.RadioButton radioQuarterly;
		private System.Windows.Forms.ContextMenuStrip contextMenuLinkedProduction;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private UI.Button butAddAdjustment;
		private UI.Button butDeleteProduction;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.CheckBox checkProductionLock;
		private System.Windows.Forms.Label labelOverpaidWarning;
	}
}
