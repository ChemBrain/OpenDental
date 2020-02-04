using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormFinanceCharges : ODForm {
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radio30;
		private System.Windows.Forms.RadioButton radio90;
		private System.Windows.Forms.RadioButton radio60;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private OpenDental.ValidNum textAPR;
		private System.ComponentModel.Container components = null;
		//private ArrayList ALPosIndices;
		private ValidDate textDateLastRun;
		private Label label5;
		private OpenDental.UI.Button butUndo;
		private GroupBox groupBox2;
		private ValidDate textDateUndo;
		private Label label6;
		private ListBox listBillType;
		private Panel panel1;
		private Label label8;
		private ValidDouble textBillingCharge;
		private RadioButton radioBillingCharge;
		private RadioButton radioFinanceCharge;
		private Label label12;
		private Label label11;
		private ValidDouble textOver;
		private ValidDouble textAtLeast;
		private Label labelOver;
		private Label labelAtLeast;
		private CheckBox checkCompound;
		private Label labelCompound;
		private Label label7;
		private CheckBox checkBadAddress;
		private GroupBox groupBoxFilters;
		private ValidDouble textExcludeLessThan;
		private Label labelExcludeBalanceLessThan;
		private CheckBox checkExcludeAccountNoTil;
		private CheckBox checkIgnoreInPerson;
		private CheckBox checkExcludeInsPending;
		private CheckBox checkExcludeInactive;
		private Label labelExcludeNotBilledSince;
		private ValidDate textExcludeNotBilledSince;
		private GroupBox groupBoxAssignCharge;
		private RadioButton radioSpecificProv;
		private RadioButton radioPatPriProv;
		private UI.ComboBoxPlus comboSpecificProv;
		private List<Def> _listBillingTypeDefs;
		///<summary>Filtered list of providers based on the current clinic--used to populate the Combo Box Providers.</summary>
		private List<Provider> _listCurClinicProviders;

		///<summary></summary>
		public FormFinanceCharges(){
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFinanceCharges));
			this.textDate = new OpenDental.ValidDate();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radio30 = new System.Windows.Forms.RadioButton();
			this.radio90 = new System.Windows.Forms.RadioButton();
			this.radio60 = new System.Windows.Forms.RadioButton();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textAPR = new OpenDental.ValidNum();
			this.textDateLastRun = new OpenDental.ValidDate();
			this.label5 = new System.Windows.Forms.Label();
			this.butUndo = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textDateUndo = new OpenDental.ValidDate();
			this.label6 = new System.Windows.Forms.Label();
			this.listBillType = new System.Windows.Forms.ListBox();
			this.label7 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelCompound = new System.Windows.Forms.Label();
			this.checkCompound = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.textOver = new OpenDental.ValidDouble();
			this.textAtLeast = new OpenDental.ValidDouble();
			this.labelOver = new System.Windows.Forms.Label();
			this.labelAtLeast = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.radioFinanceCharge = new System.Windows.Forms.RadioButton();
			this.textBillingCharge = new OpenDental.ValidDouble();
			this.radioBillingCharge = new System.Windows.Forms.RadioButton();
			this.checkBadAddress = new System.Windows.Forms.CheckBox();
			this.groupBoxFilters = new System.Windows.Forms.GroupBox();
			this.textExcludeLessThan = new OpenDental.ValidDouble();
			this.textExcludeNotBilledSince = new OpenDental.ValidDate();
			this.labelExcludeNotBilledSince = new System.Windows.Forms.Label();
			this.labelExcludeBalanceLessThan = new System.Windows.Forms.Label();
			this.checkExcludeAccountNoTil = new System.Windows.Forms.CheckBox();
			this.checkIgnoreInPerson = new System.Windows.Forms.CheckBox();
			this.checkExcludeInsPending = new System.Windows.Forms.CheckBox();
			this.checkExcludeInactive = new System.Windows.Forms.CheckBox();
			this.groupBoxAssignCharge = new System.Windows.Forms.GroupBox();
			this.comboSpecificProv = new UI.ComboBoxPlus();
			this.radioSpecificProv = new System.Windows.Forms.RadioButton();
			this.radioPatPriProv = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBoxFilters.SuspendLayout();
			this.groupBoxAssignCharge.SuspendLayout();
			this.SuspendLayout();
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(137, 42);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(78, 20);
			this.textDate.TabIndex = 15;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 46);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 14);
			this.label1.TabIndex = 20;
			this.label1.Text = "Date of new charges";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radio30);
			this.groupBox1.Controls.Add(this.radio90);
			this.groupBox1.Controls.Add(this.radio60);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(24, 226);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(167, 82);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Calculate on balances aged";
			// 
			// radio30
			// 
			this.radio30.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio30.Location = new System.Drawing.Point(13, 17);
			this.radio30.Name = "radio30";
			this.radio30.Size = new System.Drawing.Size(104, 17);
			this.radio30.TabIndex = 1;
			this.radio30.Text = "Over 30 Days";
			// 
			// radio90
			// 
			this.radio90.Checked = true;
			this.radio90.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio90.Location = new System.Drawing.Point(13, 56);
			this.radio90.Name = "radio90";
			this.radio90.Size = new System.Drawing.Size(104, 17);
			this.radio90.TabIndex = 3;
			this.radio90.TabStop = true;
			this.radio90.Text = "Over 90 Days";
			// 
			// radio60
			// 
			this.radio60.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio60.Location = new System.Drawing.Point(13, 36);
			this.radio60.Name = "radio60";
			this.radio60.Size = new System.Drawing.Size(104, 17);
			this.radio60.TabIndex = 2;
			this.radio60.Text = "Over 60 Days";
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(486, 486);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 19;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(405, 486);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 18;
			this.butOK.Text = "Run";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(67, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 14);
			this.label2.TabIndex = 22;
			this.label2.Text = "APR";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(194, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 14);
			this.label3.TabIndex = 23;
			this.label3.Text = "%";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(212, 46);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(102, 14);
			this.label4.TabIndex = 24;
			this.label4.Text = "(For Example: 18)";
			// 
			// textAPR
			// 
			this.textAPR.Location = new System.Drawing.Point(147, 43);
			this.textAPR.MaxVal = 255;
			this.textAPR.MinVal = 0;
			this.textAPR.Name = "textAPR";
			this.textAPR.Size = new System.Drawing.Size(42, 20);
			this.textAPR.TabIndex = 26;
			// 
			// textDateLastRun
			// 
			this.textDateLastRun.Location = new System.Drawing.Point(137, 16);
			this.textDateLastRun.Name = "textDateLastRun";
			this.textDateLastRun.ReadOnly = true;
			this.textDateLastRun.Size = new System.Drawing.Size(78, 20);
			this.textDateLastRun.TabIndex = 27;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 20);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(123, 14);
			this.label5.TabIndex = 28;
			this.label5.Text = "Date last run";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butUndo
			// 
			this.butUndo.Location = new System.Drawing.Point(75, 45);
			this.butUndo.Name = "butUndo";
			this.butUndo.Size = new System.Drawing.Size(78, 25);
			this.butUndo.TabIndex = 30;
			this.butUndo.Text = "Undo";
			this.butUndo.Click += new System.EventHandler(this.butUndo_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textDateUndo);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.butUndo);
			this.groupBox2.Location = new System.Drawing.Point(24, 314);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(167, 79);
			this.groupBox2.TabIndex = 31;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Undo finance/billing charges";
			// 
			// textDateUndo
			// 
			this.textDateUndo.Location = new System.Drawing.Point(75, 19);
			this.textDateUndo.Name = "textDateUndo";
			this.textDateUndo.ReadOnly = true;
			this.textDateUndo.Size = new System.Drawing.Size(78, 20);
			this.textDateUndo.TabIndex = 31;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(4, 22);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 14);
			this.label6.TabIndex = 32;
			this.label6.Text = "Date to undo";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listBillType
			// 
			this.listBillType.Location = new System.Drawing.Point(397, 34);
			this.listBillType.Name = "listBillType";
			this.listBillType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBillType.Size = new System.Drawing.Size(158, 186);
			this.listBillType.TabIndex = 32;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(397, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(158, 16);
			this.label7.TabIndex = 33;
			this.label7.Text = "Only apply to these Billing Types";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.labelCompound);
			this.panel1.Controls.Add(this.checkCompound);
			this.panel1.Controls.Add(this.label12);
			this.panel1.Controls.Add(this.label11);
			this.panel1.Controls.Add(this.textOver);
			this.panel1.Controls.Add(this.textAtLeast);
			this.panel1.Controls.Add(this.labelOver);
			this.panel1.Controls.Add(this.labelAtLeast);
			this.panel1.Controls.Add(this.label8);
			this.panel1.Controls.Add(this.radioFinanceCharge);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.textBillingCharge);
			this.panel1.Controls.Add(this.textAPR);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.radioBillingCharge);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Location = new System.Drawing.Point(24, 68);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(319, 152);
			this.panel1.TabIndex = 34;
			// 
			// labelCompound
			// 
			this.labelCompound.Location = new System.Drawing.Point(28, 124);
			this.labelCompound.Name = "labelCompound";
			this.labelCompound.Size = new System.Drawing.Size(95, 14);
			this.labelCompound.TabIndex = 39;
			this.labelCompound.Text = "Compound interest";
			this.labelCompound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkCompound
			// 
			this.checkCompound.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCompound.Checked = true;
			this.checkCompound.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkCompound.Location = new System.Drawing.Point(145, 124);
			this.checkCompound.Margin = new System.Windows.Forms.Padding(0);
			this.checkCompound.Name = "checkCompound";
			this.checkCompound.Size = new System.Drawing.Size(16, 14);
			this.checkCompound.TabIndex = 35;
			this.checkCompound.UseVisualStyleBackColor = true;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(135, 73);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(12, 14);
			this.label12.TabIndex = 38;
			this.label12.Text = "$";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(135, 100);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(12, 14);
			this.label11.TabIndex = 37;
			this.label11.Text = "$";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textOver
			// 
			this.textOver.BackColor = System.Drawing.SystemColors.Window;
			this.textOver.Location = new System.Drawing.Point(147, 97);
			this.textOver.MaxVal = 100000000D;
			this.textOver.MinVal = -100000000D;
			this.textOver.Name = "textOver";
			this.textOver.Size = new System.Drawing.Size(42, 20);
			this.textOver.TabIndex = 36;
			// 
			// textAtLeast
			// 
			this.textAtLeast.BackColor = System.Drawing.SystemColors.Window;
			this.textAtLeast.Location = new System.Drawing.Point(147, 70);
			this.textAtLeast.MaxVal = 100000000D;
			this.textAtLeast.MinVal = -100000000D;
			this.textAtLeast.Name = "textAtLeast";
			this.textAtLeast.Size = new System.Drawing.Size(42, 20);
			this.textAtLeast.TabIndex = 35;
			// 
			// labelOver
			// 
			this.labelOver.Location = new System.Drawing.Point(28, 99);
			this.labelOver.Name = "labelOver";
			this.labelOver.Size = new System.Drawing.Size(95, 14);
			this.labelOver.TabIndex = 33;
			this.labelOver.Text = "Only if over";
			this.labelOver.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelAtLeast
			// 
			this.labelAtLeast.Location = new System.Drawing.Point(28, 73);
			this.labelAtLeast.Name = "labelAtLeast";
			this.labelAtLeast.Size = new System.Drawing.Size(95, 14);
			this.labelAtLeast.TabIndex = 34;
			this.labelAtLeast.Text = "Charge at least";
			this.labelAtLeast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(135, 14);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(12, 14);
			this.label8.TabIndex = 28;
			this.label8.Text = "$";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// radioFinanceCharge
			// 
			this.radioFinanceCharge.AutoSize = true;
			this.radioFinanceCharge.Checked = true;
			this.radioFinanceCharge.Location = new System.Drawing.Point(11, 44);
			this.radioFinanceCharge.Name = "radioFinanceCharge";
			this.radioFinanceCharge.Size = new System.Drawing.Size(100, 17);
			this.radioFinanceCharge.TabIndex = 0;
			this.radioFinanceCharge.TabStop = true;
			this.radioFinanceCharge.Text = "Finance Charge";
			this.radioFinanceCharge.UseVisualStyleBackColor = true;
			this.radioFinanceCharge.CheckedChanged += new System.EventHandler(this.radioFinanceCharge_CheckedChanged);
			// 
			// textBillingCharge
			// 
			this.textBillingCharge.BackColor = System.Drawing.SystemColors.Window;
			this.textBillingCharge.Location = new System.Drawing.Point(147, 12);
			this.textBillingCharge.MaxVal = 100000000D;
			this.textBillingCharge.MinVal = -100000000D;
			this.textBillingCharge.Name = "textBillingCharge";
			this.textBillingCharge.ReadOnly = true;
			this.textBillingCharge.Size = new System.Drawing.Size(42, 20);
			this.textBillingCharge.TabIndex = 27;
			// 
			// radioBillingCharge
			// 
			this.radioBillingCharge.AutoSize = true;
			this.radioBillingCharge.Location = new System.Drawing.Point(11, 12);
			this.radioBillingCharge.Name = "radioBillingCharge";
			this.radioBillingCharge.Size = new System.Drawing.Size(89, 17);
			this.radioBillingCharge.TabIndex = 1;
			this.radioBillingCharge.TabStop = true;
			this.radioBillingCharge.Text = "Billing Charge";
			this.radioBillingCharge.UseVisualStyleBackColor = true;
			this.radioBillingCharge.CheckedChanged += new System.EventHandler(this.radioBillingCharge_CheckedChanged);
			// 
			// checkBadAddress
			// 
			this.checkBadAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBadAddress.Location = new System.Drawing.Point(6, 17);
			this.checkBadAddress.Name = "checkBadAddress";
			this.checkBadAddress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkBadAddress.Size = new System.Drawing.Size(295, 17);
			this.checkBadAddress.TabIndex = 39;
			this.checkBadAddress.Text = "Exclude bad addresses (no zip code)";
			this.checkBadAddress.UseVisualStyleBackColor = true;
			// 
			// groupBoxFilters
			// 
			this.groupBoxFilters.Controls.Add(this.textExcludeLessThan);
			this.groupBoxFilters.Controls.Add(this.textExcludeNotBilledSince);
			this.groupBoxFilters.Controls.Add(this.labelExcludeNotBilledSince);
			this.groupBoxFilters.Controls.Add(this.labelExcludeBalanceLessThan);
			this.groupBoxFilters.Controls.Add(this.checkExcludeAccountNoTil);
			this.groupBoxFilters.Controls.Add(this.checkIgnoreInPerson);
			this.groupBoxFilters.Controls.Add(this.checkExcludeInsPending);
			this.groupBoxFilters.Controls.Add(this.checkExcludeInactive);
			this.groupBoxFilters.Controls.Add(this.checkBadAddress);
			this.groupBoxFilters.Location = new System.Drawing.Point(197, 226);
			this.groupBoxFilters.Name = "groupBoxFilters";
			this.groupBoxFilters.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.groupBoxFilters.Size = new System.Drawing.Size(364, 167);
			this.groupBoxFilters.TabIndex = 40;
			this.groupBoxFilters.TabStop = false;
			this.groupBoxFilters.Text = "Finance/Billing Filters";
			// 
			// textExcludeLessThan
			// 
			this.textExcludeLessThan.BackColor = System.Drawing.SystemColors.Window;
			this.textExcludeLessThan.Location = new System.Drawing.Point(288, 116);
			this.textExcludeLessThan.MaxVal = 100000000D;
			this.textExcludeLessThan.MinVal = -100000000D;
			this.textExcludeLessThan.Name = "textExcludeLessThan";
			this.textExcludeLessThan.Size = new System.Drawing.Size(42, 20);
			this.textExcludeLessThan.TabIndex = 54;
			// 
			// textExcludeNotBilledSince
			// 
			this.textExcludeNotBilledSince.Location = new System.Drawing.Point(288, 140);
			this.textExcludeNotBilledSince.Name = "textExcludeNotBilledSince";
			this.textExcludeNotBilledSince.Size = new System.Drawing.Size(70, 20);
			this.textExcludeNotBilledSince.TabIndex = 47;
			// 
			// labelExcludeNotBilledSince
			// 
			this.labelExcludeNotBilledSince.Location = new System.Drawing.Point(6, 142);
			this.labelExcludeNotBilledSince.Name = "labelExcludeNotBilledSince";
			this.labelExcludeNotBilledSince.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.labelExcludeNotBilledSince.Size = new System.Drawing.Size(281, 16);
			this.labelExcludeNotBilledSince.TabIndex = 48;
			this.labelExcludeNotBilledSince.Text = "Exclude accounts not billed since";
			this.labelExcludeNotBilledSince.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelExcludeBalanceLessThan
			// 
			this.labelExcludeBalanceLessThan.Location = new System.Drawing.Point(6, 118);
			this.labelExcludeBalanceLessThan.Name = "labelExcludeBalanceLessThan";
			this.labelExcludeBalanceLessThan.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.labelExcludeBalanceLessThan.Size = new System.Drawing.Size(281, 16);
			this.labelExcludeBalanceLessThan.TabIndex = 53;
			this.labelExcludeBalanceLessThan.Text = "Exclude if balance is less than";
			this.labelExcludeBalanceLessThan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkExcludeAccountNoTil
			// 
			this.checkExcludeAccountNoTil.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeAccountNoTil.Location = new System.Drawing.Point(6, 97);
			this.checkExcludeAccountNoTil.Name = "checkExcludeAccountNoTil";
			this.checkExcludeAccountNoTil.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkExcludeAccountNoTil.Size = new System.Drawing.Size(295, 17);
			this.checkExcludeAccountNoTil.TabIndex = 44;
			this.checkExcludeAccountNoTil.Text = "Exclude accounts (guarantor) without Truth in Lending";
			this.checkExcludeAccountNoTil.UseVisualStyleBackColor = true;
			// 
			// checkIgnoreInPerson
			// 
			this.checkIgnoreInPerson.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIgnoreInPerson.Location = new System.Drawing.Point(6, 77);
			this.checkIgnoreInPerson.Name = "checkIgnoreInPerson";
			this.checkIgnoreInPerson.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkIgnoreInPerson.Size = new System.Drawing.Size(295, 17);
			this.checkIgnoreInPerson.TabIndex = 43;
			this.checkIgnoreInPerson.Text = "Ignore walkout (In person) Statements";
			this.checkIgnoreInPerson.UseVisualStyleBackColor = true;
			// 
			// checkExcludeInsPending
			// 
			this.checkExcludeInsPending.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeInsPending.Location = new System.Drawing.Point(6, 57);
			this.checkExcludeInsPending.Name = "checkExcludeInsPending";
			this.checkExcludeInsPending.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkExcludeInsPending.Size = new System.Drawing.Size(295, 17);
			this.checkExcludeInsPending.TabIndex = 41;
			this.checkExcludeInsPending.Text = "Exclude if insurance pending";
			this.checkExcludeInsPending.UseVisualStyleBackColor = true;
			// 
			// checkExcludeInactive
			// 
			this.checkExcludeInactive.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeInactive.Location = new System.Drawing.Point(6, 37);
			this.checkExcludeInactive.Name = "checkExcludeInactive";
			this.checkExcludeInactive.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkExcludeInactive.Size = new System.Drawing.Size(295, 17);
			this.checkExcludeInactive.TabIndex = 40;
			this.checkExcludeInactive.Text = "Exclude inactive families";
			this.checkExcludeInactive.UseVisualStyleBackColor = true;
			// 
			// groupBoxAssignCharge
			// 
			this.groupBoxAssignCharge.Controls.Add(this.comboSpecificProv);
			this.groupBoxAssignCharge.Controls.Add(this.radioSpecificProv);
			this.groupBoxAssignCharge.Controls.Add(this.radioPatPriProv);
			this.groupBoxAssignCharge.Location = new System.Drawing.Point(24, 399);
			this.groupBoxAssignCharge.Name = "groupBoxAssignCharge";
			this.groupBoxAssignCharge.Size = new System.Drawing.Size(301, 76);
			this.groupBoxAssignCharge.TabIndex = 49;
			this.groupBoxAssignCharge.TabStop = false;
			this.groupBoxAssignCharge.Text = "Assign charges to:";
			// 
			// comboSpecificProv
			// 
			this.comboSpecificProv.Location = new System.Drawing.Point(121, 45);
			this.comboSpecificProv.Name = "comboSpecificProv";
			this.comboSpecificProv.Size = new System.Drawing.Size(174, 21);
			this.comboSpecificProv.TabIndex = 4;
			// 
			// radioSpecificProv
			// 
			this.radioSpecificProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioSpecificProv.Location = new System.Drawing.Point(13, 47);
			this.radioSpecificProv.Name = "radioSpecificProv";
			this.radioSpecificProv.Size = new System.Drawing.Size(105, 17);
			this.radioSpecificProv.TabIndex = 1;
			this.radioSpecificProv.Text = "Specific Provider";
			this.radioSpecificProv.UseVisualStyleBackColor = true;
			// 
			// radioPatPriProv
			// 
			this.radioPatPriProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioPatPriProv.Location = new System.Drawing.Point(13, 24);
			this.radioPatPriProv.Name = "radioPatPriProv";
			this.radioPatPriProv.Size = new System.Drawing.Size(144, 17);
			this.radioPatPriProv.TabIndex = 0;
			this.radioPatPriProv.Text = "Patient\'s Primary Provider";
			this.radioPatPriProv.UseVisualStyleBackColor = true;
			this.radioPatPriProv.CheckedChanged += new System.EventHandler(this.RadioPatPriProv_CheckedChanged);
			// 
			// FormFinanceCharges
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(573, 523);
			this.Controls.Add(this.groupBoxAssignCharge);
			this.Controls.Add(this.groupBoxFilters);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.listBillType);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.textDateLastRun);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(578, 514);
			this.Name = "FormFinanceCharges";
			this.ShowInTaskbar = false;
			this.Text = "Finance/Billing Charges";
			this.Load += new System.EventHandler(this.FormFinanceCharges_Load);
			this.Shown += new System.EventHandler(this.FormFinanceCharges_Shown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBoxFilters.ResumeLayout(false);
			this.groupBoxFilters.PerformLayout();
			this.groupBoxAssignCharge.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormFinanceCharges_Load(object sender, System.EventArgs e) {
			if(PrefC.GetLong(PrefName.FinanceChargeAdjustmentType)==0){
				MsgBox.Show(this,"No finance charge adjustment type has been set.  Please go to Setup | Account to fix this.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(PrefC.GetLong(PrefName.BillingChargeAdjustmentType)==0){
				MsgBox.Show(this,"No billing charge adjustment type has been set.  Please go to Setup | Account to fix this.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			_listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			if(_listBillingTypeDefs.Count==0){//highly unlikely that this would happen
				MsgBox.Show(this,"No billing types have been set up or are visible.");
				DialogResult=DialogResult.Cancel;
				return;
			}
		}

		///<summary>The following logic was moved into a Shown method, rather than a Load method, because the progress bar causes the 
		///window to popbehind FormOpenDental when in Load.</summary>
		private void FormFinanceCharges_Shown(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				if(!RunAgingEnterprise(true)) {
					DialogResult=DialogResult.Cancel;
					return;
				}
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.AgingRan,0,"Aging Ran Automatically - Finance Charges Form");
				DateTime asOfDate=(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)?PrefC.GetDate(PrefName.DateLastAging):DateTime.Today);
				bool result=true;
				Cursor=Cursors.WaitCursor;
				ODProgress.ShowAction(() => {
						Ledgers.RunAging();
					},
					startingMessage:Lan.g(this,"Calculating aging for all patients as of")+" "+asOfDate.ToShortDateString()+"...",
					actionException:ex => {
						Ledgers.AgingExceptionHandler(ex,this);
						result=false;
					});
				Cursor=Cursors.Default;
				if(!result) {//if aging failed
					return;
				}
			}
			textDate.Text=DateTime.Today.ToShortDateString();		
			textAPR.MaxVal=100;
			textAPR.MinVal=0;
			textAPR.Text=PrefC.GetString(PrefName.FinanceChargeAPR);
			textBillingCharge.Text=PrefC.GetString(PrefName.BillingChargeAmount);
			for(int i=0;i<_listBillingTypeDefs.Count;i++) {
				listBillType.Items.Add(_listBillingTypeDefs[i].ItemName);
				listBillType.SetSelected(i,true);
			}
			string defaultChargeMethod = PrefC.GetString(PrefName.BillingChargeOrFinanceIsDefault);
			if (defaultChargeMethod == "Finance") {
				radioFinanceCharge.Checked = true;
				textDateLastRun.Text = PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
				textDateUndo.Text = PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
				textBillingCharge.ReadOnly=true;
				textBillingCharge.BackColor=System.Drawing.SystemColors.Control;
			}
			else if (defaultChargeMethod == "Billing") {
				radioBillingCharge.Checked = true;
				textDateLastRun.Text = PrefC.GetDate(PrefName.BillingChargeLastRun).ToShortDateString();
				textDateUndo.Text = PrefC.GetDate(PrefName.BillingChargeLastRun).ToShortDateString();
			}
			textAtLeast.Text=PrefC.GetString(PrefName.FinanceChargeAtLeast);
			textOver.Text=PrefC.GetString(PrefName.FinanceChargeOnlyIfOver);
			textExcludeNotBilledSince.Text=GetFinanceBillingLastRun().ToShortDateString();
			comboSpecificProv.Items.AddProvsAbbr(Providers.GetProvsForClinic(Clinics.ClinicNum));
			comboSpecificProv.SelectedIndex=0;
			radioPatPriProv.Checked=true;
		}

		///<summary>If !isPreCharges, a message box will display for any errors instructing users to try again.  If the failed aging attempt is after
		///charges have been added/deleted, we don't want to inform the user that the transaction failed so run again since the charges were successfully
		///inserted/deleted and it was only updating the aged balances that failed.  If isPreCharges, this won't run aging again if the last aging run was
		///today.  If !isPreCharges, we will run aging even if it was run today to update aged bals to include the charges added/deleted.</summary>
		private bool RunAgingEnterprise(bool isOnLoad=false) {
			DateTime dtNow=MiscData.GetNowDateTime();
			DateTime dtToday=dtNow.Date;
			DateTime dateLastAging=PrefC.GetDate(PrefName.DateLastAging);
			if(isOnLoad && dateLastAging.Date==dtToday) {
				return true;//this is prior to inserting/deleting charges and aging has already been run for this date
			}
			Prefs.RefreshCache();
			DateTime dateTAgingBeganPref=PrefC.GetDateT(PrefName.AgingBeginDateTime);
			if(dateTAgingBeganPref>DateTime.MinValue) {
				if(isOnLoad) {
					MessageBox.Show(this,Lan.g(this,"In order to add finance charges, aging must be calculated, but you cannot run aging until it has finished "
						+"the current calculations which began on")+" "+dateTAgingBeganPref.ToString()+".\r\n"+Lans.g(this,"If you believe the current aging "
						+"process has finished, a user with SecurityAdmin permission can manually clear the date and time by going to Setup | Miscellaneous and "
						+"pressing the 'Clear' button."));
				}
				return false;
			}
			SecurityLogs.MakeLogEntry(Permissions.AgingRan,0,"Aging Ran - Finance Charges Form");
			Prefs.UpdateString(PrefName.AgingBeginDateTime,POut.DateT(dtNow,false));//get lock on pref to block others
			Signalods.SetInvalid(InvalidType.Prefs);//signal a cache refresh so other computers will have the updated pref as quickly as possible
			bool result=true;
			Cursor=Cursors.WaitCursor;
			ODProgress.ShowAction(() => {
					Ledgers.ComputeAging(0,dtToday);
					Prefs.UpdateString(PrefName.DateLastAging,POut.Date(dtToday,false));
				},
				startingMessage:Lan.g(this,"Calculating enterprise aging for all patients as of")+" "+dtToday.ToShortDateString()+"...",
				actionException:ex => {
					Ledgers.AgingExceptionHandler(ex,this,isOnLoad);
					result=false;
				});
			Cursor=Cursors.Default;
			Prefs.UpdateString(PrefName.AgingBeginDateTime,"");//clear lock on pref whether aging was successful or not
			return result;
		}

		/// <summary>Gets an Aging List from the Finance/Billing Charges with the filter settings from the UI.</summary>
		private List<PatAging> GetFinanceBillingAgingList() {
			List<long> listSelectedBillTypes=listBillType.SelectedIndices.OfType<int>().Select(x => _listBillingTypeDefs[x].DefNum).ToList();
			SerializableDictionary<long,PatAgingData> dictPatAgingData=AgingData.GetAgingData(false,true,checkExcludeInsPending.Checked,false,false,new List<long>());
			List<long> listPendingInsPatNums=new List<long>();
			if(checkExcludeInsPending.Checked) { //Only fill list if excluding pending ins
				foreach(KeyValuePair<long,PatAgingData> kvp in dictPatAgingData) {
					if(kvp.Value.HasPendingIns) {
						listPendingInsPatNums.Add(kvp.Key);
					}
				}
			}
			string age="";
			if(radio30.Checked) {
				age="30";
			}
			else if(radio60.Checked) {
				age="60";
			}
			else if(radio90.Checked) {
				age="90";
			}
			DateTime lastStatement=PIn.Date(textExcludeNotBilledSince.Text);
			bool hasFilterSinceLastStatement=true;
			//If the 'exclude accounts not billed since' date has been removed, select the latest Billing/Finance Statement date. 
			if(textExcludeNotBilledSince.Text=="") {
				lastStatement=GetFinanceBillingLastRun();
				//Because the user did not specify a filter date, we are NOT going to exclude accounts not billed since lastStatement.
				hasFilterSinceLastStatement=false;
			}
			bool excludeNegativeCredits=false;
			bool isSuperStatements=false;
			bool isSinglePatient=false;
			return Patients.GetAgingList(age,lastStatement,listSelectedBillTypes,checkBadAddress.Checked,excludeNegativeCredits,PIn.Double(textExcludeLessThan.Text)
				,checkExcludeInactive.Checked,checkIgnoreInPerson.Checked,new List<long>(),isSuperStatements,isSinglePatient,listPendingInsPatNums,
				new List<long>(),new SerializableDictionary<long,List<PatAgingTransaction>>(),checkExcludeAccountNoTil.Checked,hasFilterSinceLastStatement,true);
		}

		///<summary>Returns the date the Finance or Billing was last run depending on the currently selected preference.</summary>
		private DateTime GetFinanceBillingLastRun() {
			if(PrefC.GetString(PrefName.BillingChargeOrFinanceIsDefault)=="Finance") {
				return PrefC.GetDate(PrefName.FinanceChargeLastRun);
			}
			else {
				return PrefC.GetDate(PrefName.BillingChargeLastRun);
			}
		}

		///<summary>The Treating Provider radio button is clicked: disable the provider combo box and picker.
		///This setting will run the Finance/Billing Charge on the primary provider (PriProv)</summary>
		private void RadioPatPriProv_CheckedChanged(object sender,EventArgs e) {
			if(radioPatPriProv.Checked) {
				comboSpecificProv.Enabled=false;
			}
			else {
				comboSpecificProv.Enabled=true;
			}
		}

		private void radioFinanceCharge_CheckedChanged(object sender, EventArgs e) {
			textAPR.ReadOnly = false;
			textAPR.BackColor = System.Drawing.SystemColors.Window;
			textAtLeast.ReadOnly=false;
			textAtLeast.BackColor = System.Drawing.SystemColors.Window;
			labelAtLeast.Enabled=true;
			textOver.ReadOnly=false;
			textOver.BackColor = System.Drawing.SystemColors.Window;
			labelOver.Enabled=true;
			textBillingCharge.ReadOnly = true;
			textBillingCharge.BackColor = System.Drawing.SystemColors.Control;
			textDateLastRun.Text = PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
			textDateUndo.Text = PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
		}

		private void radioBillingCharge_CheckedChanged(object sender, EventArgs e) {
			textAPR.ReadOnly = true;
			textAPR.BackColor = System.Drawing.SystemColors.Control;
			textAtLeast.ReadOnly=true;
			textAtLeast.BackColor = System.Drawing.SystemColors.Control;
			labelAtLeast.Enabled=false;
			textOver.ReadOnly=true;
			textOver.BackColor = System.Drawing.SystemColors.Control;
			labelOver.Enabled=false;
			textBillingCharge.ReadOnly = false;
			textBillingCharge.BackColor = System.Drawing.SystemColors.Window;
			textDateLastRun.Text = PrefC.GetDate(PrefName.BillingChargeLastRun).ToShortDateString();
			textDateUndo.Text = PrefC.GetDate(PrefName.BillingChargeLastRun).ToShortDateString();
		}

		private void butUndo_Click(object sender,EventArgs e) {
			string chargeType=(radioFinanceCharge.Checked?"Finance":"Billing");
			if(MessageBox.Show(Lan.g(this,"Undo all "+chargeType.ToLower()+" charges for")+" "+textDateUndo.Text+"?","",MessageBoxButtons.OKCancel)
				!=DialogResult.OK)
			{
				return;
			}
			int rowsAffected=0;
			Cursor=Cursors.WaitCursor;
			DateTime dateUndo=PIn.Date(textDateUndo.Text);
			bool billingCharge=radioBillingCharge.Checked;
			ODProgress.ShowAction(() => rowsAffected=(int)Adjustments.UndoFinanceOrBillingCharges(dateUndo,billingCharge),
				startingMessage:Lan.g(this,"Deleting "+chargeType.ToLower()+" charge adjustments")+"...",
				eventType:typeof(BillingEvent),
				odEventType:ODEventType.Billing);
			Cursor=Cursors.Default;
			MessageBox.Show(Lan.g(this,chargeType+" charge adjustments deleted")+": "+rowsAffected);
			if(rowsAffected==0) {
				DialogResult=DialogResult.OK;
				return;
			}
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				if(!RunAgingEnterprise()) {
					MsgBox.Show(this,"There was an error calculating aging after the "+chargeType.ToLower()+" charge adjustments were deleted.\r\n"
						+"You should run aging later to update affected accounts.");
				}
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.AgingRan,0,"Aging Ran - Finance Charges Form");
				DateTime asOfDate=(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)?PrefC.GetDate(PrefName.DateLastAging):DateTime.Today);
				Cursor=Cursors.WaitCursor;
				ODProgress.ShowAction(
					() => {
						Ledgers.RunAging();
					},
					startingMessage:Lan.g(this,"Calculating aging for all patients as of")+" "+asOfDate.ToShortDateString()+"...",
					actionException:ex => Ledgers.AgingExceptionHandler(ex,this));
				Cursor=Cursors.Default;
			}
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,chargeType+" Charges undo. Date "+textDateUndo.Text);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!=""
				|| textAPR.errorProvider1.GetError(textAPR)!=""
				|| textAtLeast.errorProvider1.GetError(textAtLeast)!=""
				|| textOver.errorProvider1.GetError(textOver)!=""
				|| textExcludeLessThan.errorProvider1.GetError(textExcludeLessThan)!=""
				|| textExcludeNotBilledSince.errorProvider1.GetError(textExcludeNotBilledSince)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			DateTime date=PIn.Date(textDate.Text);
			if(PrefC.GetDate(PrefName.FinanceChargeLastRun).AddDays(25)>date) {
				if(!MsgBox.Show(this,true,"Warning.  Finance charges should not be run more than once per month.  Continue?")) {
					return;
				}
			} 
			else if(PrefC.GetDate(PrefName.BillingChargeLastRun).AddDays(25)>date) {
				if(!MsgBox.Show(this,true,"Warning.  Billing charges should not be run more than once per month.  Continue?")) {
					return;
				}
			}
			if(listBillType.SelectedIndices.Count==0) {
				MsgBox.Show(this,"Please select at least one billing type first.");
				return;
			}
			if(PIn.Long(textAPR.Text) < 2) {
				if(!MsgBox.Show(this,true,"The APR is much lower than normal. Do you wish to proceed?")) {
					return;
				}
			}
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily) && PrefC.GetDate(PrefName.DateLastAging).AddMonths(1)<=DateTime.Today) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"It has been more than a month since aging has been run.  It is recommended that you update the "
					+"aging date and run aging before continuing."))
				{
					return;
				}
				//we might also consider a warning if textDate.Text does not match DateLastAging.  Probably not needed for daily aging, though.
			}
			string chargeType=(radioFinanceCharge.Checked?"Finance":"Billing");//For display only
			Action actionCloseProgress=null;
			int chargesAdded=0;
			try {
				actionCloseProgress=ODProgress.Show(ODEventType.Billing,typeof(BillingEvent),Lan.g(this,"Gathering patients with aged balances")+"...");
				List<PatAging> listPatAgings=GetFinanceBillingAgingList();//Get the Aging List for Finance and Billing
				long adjType=PrefC.GetLong(PrefName.FinanceChargeAdjustmentType);
				Dictionary<long,List<Adjustment>> dictPatAdjustments=new Dictionary<long, List<Adjustment>>();
				if(!checkCompound.Checked) {
					int daysOver=(radio30.Checked ? 30
						: radio60.Checked ? 60
						: 90);
					DateTime maxAdjDate=MiscData.GetNowDateTime().Date.AddDays(-daysOver);
					dictPatAdjustments=Adjustments.GetAdjustForPatsByType(listPatAgings.Select(x => x.PatNum).ToList(),adjType,maxAdjDate);
				}
				bool isRadio60Checked=radio60.Checked;
				bool isRadio30Checked=radio30.Checked;
				bool isRadioBillingChargeChecked=radioBillingCharge.Checked;
				string billingChargeText=textBillingCharge.Text;
				string aprText=textAPR.Text;
				string atLeastText=textAtLeast.Text;
				string overText=textOver.Text;
				List<long> listPatNums=listPatAgings.Select(x => x.PatNum).ToList();
				Dictionary<long,InstallmentPlan> dictFamilyInstallmentPlans=null;
				if(!isRadioBillingChargeChecked) {//Finance charge
					dictFamilyInstallmentPlans=InstallmentPlans.GetForFams(listPatNums);
				}
				int chargesProcessed=0;
				List<Action> listActions=new List<Action>();
				foreach(PatAging patAgingCur in listPatAgings) {
					listActions.Add(new Action(() => {
						if(++chargesProcessed%5==0) {
							BillingEvent.Fire(ODEventType.Billing,Lan.g(this,"Processing "+chargeType+" charges")+": "+chargesProcessed+" out of "
								+listPatAgings.Count);
						}
						//This WILL NOT be the same as the patient's total balance. Start with BalOver90 since all options include that bucket. Add others if needed.
						double overallBalance=patAgingCur.BalOver90+(isRadio60Checked ? patAgingCur.Bal_61_90
							: isRadio30Checked ? (patAgingCur.Bal_31_60+patAgingCur.Bal_61_90)
							: 0);
						if(overallBalance<=.01d) {
							return;
						}
						long provNumToAssignCharges;
						if(radioSpecificProv.Checked) {
							long provNum=comboSpecificProv.GetSelectedProvNum();
							provNumToAssignCharges=comboSpecificProv.GetSelectedProvNum();
						}
						else {
							provNumToAssignCharges=patAgingCur.PriProv;
						}
						if(isRadioBillingChargeChecked) {
							AddBillingCharge(patAgingCur.PatNum,date,billingChargeText,provNumToAssignCharges);
						}
						else {//Finance charge
							if(dictPatAdjustments.ContainsKey(patAgingCur.PatNum)) {//Only contains key if checkCompound is not checked.
								overallBalance-=dictPatAdjustments[patAgingCur.PatNum].Sum(x => x.AdjAmt);//Dict always contains patNum as key, but list can be empty.
							}
							if(!AddFinanceCharge(patAgingCur.PatNum,date,aprText,atLeastText,overText,overallBalance,
								provNumToAssignCharges,adjType,dictFamilyInstallmentPlans))
							{
								return;
							}
						}
						chargesAdded++;
					}));
				}
				ODThread.RunParallel(listActions,TimeSpan.FromMinutes(2));//each group of actions gets X minutes.
				if(radioFinanceCharge.Checked) {
					if(Prefs.UpdateString(PrefName.FinanceChargeAPR,textAPR.Text) 
						| Prefs.UpdateString(PrefName.FinanceChargeLastRun,POut.Date(date,false))
						| Prefs.UpdateString(PrefName.FinanceChargeAtLeast,textAtLeast.Text)
						| Prefs.UpdateString(PrefName.FinanceChargeOnlyIfOver,textOver.Text)
						| Prefs.UpdateString(PrefName.BillingChargeOrFinanceIsDefault,"Finance"))
					{
						DataValid.SetInvalid(InvalidType.Prefs);
					}
				}
				else if(radioBillingCharge.Checked) {
					if(Prefs.UpdateString(PrefName.BillingChargeAmount,textBillingCharge.Text)
						| Prefs.UpdateString(PrefName.BillingChargeLastRun,POut.Date(date,false))
						| Prefs.UpdateString(PrefName.BillingChargeOrFinanceIsDefault,"Billing"))
					{
						DataValid.SetInvalid(InvalidType.Prefs);
					}
				}
			}
			finally {
				actionCloseProgress?.Invoke();//terminates progress bar
			}
			MessageBox.Show(Lan.g(this,chargeType+" charges added")+": "+chargesAdded);
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				if(!RunAgingEnterprise()) {
					MsgBox.Show(this,"There was an error calculating aging after the "+chargeType.ToLower()+" charge adjustments were added.\r\n"
						+"You should run aging later to update affected accounts.");
				}
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.AgingRan,0,"Aging Ran - Finance Charges Form");
				DateTime asOfDate=(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)?PrefC.GetDate(PrefName.DateLastAging):DateTime.Today);
				Cursor=Cursors.WaitCursor;
				ODProgress.ShowAction(() => Ledgers.RunAging(),
					startingMessage:Lan.g(this,"Calculating aging for all patients as of")+" "+asOfDate.ToShortDateString()+"...",
					actionException:ex => Ledgers.AgingExceptionHandler(ex,this));
				Cursor=Cursors.Default;
			}
			DialogResult = DialogResult.OK;
		}

		/// <summary>Returns true if a finance charge is added, false if one is not added</summary>
		private bool AddFinanceCharge(long patNum,DateTime date,string APR,string atLeast,string ifOver,double OverallBalance,long PriProv,long adjType,
			Dictionary<long,InstallmentPlan> dictInstallmentPlans=null)
		{
			if(date.Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Adjustments cannot be made for future dates. Finance charge was not added.");
				return false;
			}
			if(dictInstallmentPlans!=null) {
				dictInstallmentPlans.TryGetValue(patNum,out InstallmentPlan installmentPlan);
				if(installmentPlan!=null) {//Patient has an installment plan so use that APR instead.
					APR=installmentPlan.APR.ToString();
				}
			}
			Adjustment AdjustmentCur = new Adjustment();
			AdjustmentCur.PatNum = patNum;
			//AdjustmentCur.DateEntry=PIn.PDate(textDate.Text);//automatically handled
			AdjustmentCur.AdjDate = date;
			AdjustmentCur.ProcDate = date;
			AdjustmentCur.AdjType = adjType;
			AdjustmentCur.AdjNote = "";//"Finance Charge";
			AdjustmentCur.AdjAmt = Math.Round(((PIn.Double(APR) * .01d / 12d) * OverallBalance),2);
			if(AdjustmentCur.AdjAmt.IsZero() || AdjustmentCur.AdjAmt<PIn.Double(ifOver)) {
				//Don't add the charge if it is less than FinanceChargeOnlyIfOver; if the charge is exactly equal to FinanceChargeOnlyIfOver,
				//the charge will be added. Ex., AdjAmt=2.00 and FinanceChargeOnlyIfOver=2.00, the charge will be added.
				//Unless AdjAmt=0.00, in which case don't add a $0.00 finance charge
				return false;
			}
			//Add an amount that is at least the amount of FinanceChargeAtLeast 
			AdjustmentCur.AdjAmt=Math.Max(AdjustmentCur.AdjAmt,PIn.Double(atLeast));
			AdjustmentCur.ProvNum = PriProv;
			Adjustments.Insert(AdjustmentCur);
			TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(AdjustmentCur);
			return true;
		}

		private void AddBillingCharge(long patNum,DateTime date,string BillingChargeAmount,long PriProv) {
			if(date.Date > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Adjustments cannot be made for future dates");
				return;
			}
			Adjustment AdjustmentCur = new Adjustment();
			AdjustmentCur.PatNum = patNum;
			//AdjustmentCur.DateEntry=PIn.PDate(textDate.Text);//automatically handled
			AdjustmentCur.AdjDate = date;
			AdjustmentCur.ProcDate = date;
			AdjustmentCur.AdjType = PrefC.GetLong(PrefName.BillingChargeAdjustmentType);
			AdjustmentCur.AdjNote = "";//"Billing Charge";
			AdjustmentCur.AdjAmt = PIn.Double(BillingChargeAmount);
			AdjustmentCur.ProvNum = PriProv;
			Adjustments.Insert(AdjustmentCur);
			TsiTransLogs.CheckAndInsertLogsIfAdjTypeExcluded(AdjustmentCur);
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
