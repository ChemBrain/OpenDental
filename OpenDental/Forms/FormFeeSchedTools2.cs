using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using CodeBase;
using OpenDentBusiness;
using OpenDental.Thinfinity;
using OpenDental.UI;

namespace OpenDental {
	///<summary></summary>
	public class FormFeeSchedTools2 :ODForm {
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butCopy;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.ComboBox comboFeeSchedTo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox groupBox2;
		private OpenDental.UI.Button butClear;
		private System.Windows.Forms.GroupBox groupBox3;
		private OpenDental.UI.Button butIncrease;
		private System.Windows.Forms.TextBox textPercent;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton radioDollar;
		private System.Windows.Forms.RadioButton radioDime;
		private System.Windows.Forms.RadioButton radioPenny;
		private GroupBox groupBox5;
		private OpenDental.UI.Button butExport;
		private OpenDental.UI.Button butImport;
		private GroupBox groupGlobalUpdateFees;
		private Label label4;
		private OpenDental.UI.Button butUpdate;
		private Label label5;
		private UI.Button butImportCanada;
		private GroupBox groupBox7;
		private ComboBox comboProvider;
		private ComboBox comboClinic;
		private ComboBox comboFeeSched;
		///<summary>The defNum of the fee schedule that is currently displayed in the main window.</summary>
		private long _schedNum;
		private List<FeeSched> _listFeeScheds;
		private List<Provider> _listProvs;
		private List<Clinic> _listClinics;
		private Label label12;
		private Label label10;
		private Label label8;
		private UI.Button butPickProv;
		private UI.Button butPickClinic;
		private UI.Button butPickSched;
		private Label label7;
		private UI.Button butPickProvTo;
		private ComboBox comboProviderTo;
		private UI.ComboBoxClinicPicker comboClinicTo;
		private UI.Button butPickSchedTo;
		private Label label9;
		private UI.Button butUpdateWriteoffs;
		private UI.ComboBoxClinicPicker comboGlobalUpdateClinics;
		private Label label11;
		private CheckBox checkShowGroups;
		private ComboBox comboGroup;
		private ComboBox comboGroupTo;
		private Label label6;
		private UI.Button butPickGroupTo;

		///<summary>A list of security logs that should be inserted.</summary>
		private List<string> _listSecurityLogEntries=new List<string>();

		///<summary>Supply the fee schedule num(DefNum) to which all these changes will apply</summary>
		public FormFeeSchedTools2(long schedNum,List<FeeSched> listFeeScheds,List<Provider> listProvs,List<Clinic> listClinics) {
			// Required for Windows Form Designer support
			InitializeComponent();
			Lan.F(this);
			_schedNum=schedNum;
			_listFeeScheds=listFeeScheds;
			_listProvs=listProvs;
			_listClinics=listClinics;//this was just simply Clinics.GetForUserod(Security.CurUser) from the parent form
		}

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeeSchedTools2));
			this.checkShowGroups = new System.Windows.Forms.CheckBox();
			this.comboClinicTo = new OpenDental.UI.ComboBoxClinicPicker();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.comboProvider = new System.Windows.Forms.ComboBox();
			this.comboGroup = new System.Windows.Forms.ComboBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.comboFeeSched = new System.Windows.Forms.ComboBox();
			this.butPickProv = new OpenDental.UI.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.butPickClinic = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.butPickSched = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.groupGlobalUpdateFees = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.butUpdateWriteoffs = new OpenDental.UI.Button();
			this.comboGlobalUpdateClinics = new OpenDental.UI.ComboBoxClinicPicker();
			this.label4 = new System.Windows.Forms.Label();
			this.butUpdate = new OpenDental.UI.Button();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.butImportCanada = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.butExport = new OpenDental.UI.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.radioPenny = new System.Windows.Forms.RadioButton();
			this.radioDime = new System.Windows.Forms.RadioButton();
			this.radioDollar = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.butIncrease = new OpenDental.UI.Button();
			this.textPercent = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.butClear = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butPickGroupTo = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.comboGroupTo = new System.Windows.Forms.ComboBox();
			this.comboProviderTo = new System.Windows.Forms.ComboBox();
			this.comboFeeSchedTo = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.butPickProvTo = new OpenDental.UI.Button();
			this.butPickSchedTo = new OpenDental.UI.Button();
			this.butCopy = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox7.SuspendLayout();
			this.groupGlobalUpdateFees.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkShowGroups
			// 
			this.checkShowGroups.Location = new System.Drawing.Point(12, 8);
			this.checkShowGroups.Name = "checkShowGroups";
			this.checkShowGroups.Size = new System.Drawing.Size(173, 17);
			this.checkShowGroups.TabIndex = 40;
			this.checkShowGroups.Text = "Show Fee Schedule Groups";
			this.checkShowGroups.UseVisualStyleBackColor = true;
			this.checkShowGroups.Visible = false;
			this.checkShowGroups.CheckedChanged += new System.EventHandler(this.checkShowGroups_CheckedChanged);
			// 
			// comboClinicTo
			// 
			this.comboClinicTo.HqDescription = "Default";
			this.comboClinicTo.IncludeUnassigned = true;
			this.comboClinicTo.Location = new System.Drawing.Point(63, 45);
			this.comboClinicTo.Name = "comboClinicTo";
			this.comboClinicTo.SelectionModeMulti = true;
			this.comboClinicTo.Size = new System.Drawing.Size(210, 21);
			this.comboClinicTo.TabIndex = 2;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.comboProvider);
			this.groupBox7.Controls.Add(this.comboGroup);
			this.groupBox7.Controls.Add(this.comboClinic);
			this.groupBox7.Controls.Add(this.comboFeeSched);
			this.groupBox7.Controls.Add(this.butPickProv);
			this.groupBox7.Controls.Add(this.label12);
			this.groupBox7.Controls.Add(this.butPickClinic);
			this.groupBox7.Controls.Add(this.label10);
			this.groupBox7.Controls.Add(this.butPickSched);
			this.groupBox7.Controls.Add(this.label8);
			this.groupBox7.Location = new System.Drawing.Point(12, 31);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(328, 107);
			this.groupBox7.TabIndex = 0;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Select Fees";
			// 
			// comboProvider
			// 
			this.comboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProvider.FormattingEnabled = true;
			this.comboProvider.Location = new System.Drawing.Point(100, 72);
			this.comboProvider.Name = "comboProvider";
			this.comboProvider.Size = new System.Drawing.Size(174, 21);
			this.comboProvider.TabIndex = 4;
			// 
			// comboGroup
			// 
			this.comboGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGroup.FormattingEnabled = true;
			this.comboGroup.Location = new System.Drawing.Point(-160, 43);
			this.comboGroup.Name = "comboGroup";
			this.comboGroup.Size = new System.Drawing.Size(174, 21);
			this.comboGroup.TabIndex = 41;
			this.comboGroup.Visible = false;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.FormattingEnabled = true;
			this.comboClinic.Location = new System.Drawing.Point(100, 46);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(174, 21);
			this.comboClinic.TabIndex = 2;
			// 
			// comboFeeSched
			// 
			this.comboFeeSched.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched.FormattingEnabled = true;
			this.comboFeeSched.Location = new System.Drawing.Point(100, 19);
			this.comboFeeSched.Name = "comboFeeSched";
			this.comboFeeSched.Size = new System.Drawing.Size(174, 21);
			this.comboFeeSched.TabIndex = 0;
			this.comboFeeSched.SelectionChangeCommitted += new System.EventHandler(this.comboFeeCombos_SelectionChangeCommitted);
			// 
			// butPickProv
			// 
			this.butPickProv.Location = new System.Drawing.Point(279, 72);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(23, 21);
			this.butPickProv.TabIndex = 5;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(14, 20);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(87, 17);
			this.label12.TabIndex = 35;
			this.label12.Text = "Fee Schedule";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickClinic
			// 
			this.butPickClinic.Location = new System.Drawing.Point(279, 46);
			this.butPickClinic.Name = "butPickClinic";
			this.butPickClinic.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic.TabIndex = 3;
			this.butPickClinic.Text = "...";
			this.butPickClinic.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(11, 47);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(90, 17);
			this.label10.TabIndex = 34;
			this.label10.Text = "Clinic";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickSched
			// 
			this.butPickSched.Location = new System.Drawing.Point(279, 19);
			this.butPickSched.Name = "butPickSched";
			this.butPickSched.Size = new System.Drawing.Size(23, 21);
			this.butPickSched.TabIndex = 1;
			this.butPickSched.Text = "...";
			this.butPickSched.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(11, 73);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(90, 17);
			this.label8.TabIndex = 33;
			this.label8.Text = "Provider";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupGlobalUpdateFees
			// 
			this.groupGlobalUpdateFees.Controls.Add(this.label9);
			this.groupGlobalUpdateFees.Controls.Add(this.butUpdateWriteoffs);
			this.groupGlobalUpdateFees.Controls.Add(this.comboGlobalUpdateClinics);
			this.groupGlobalUpdateFees.Controls.Add(this.label4);
			this.groupGlobalUpdateFees.Controls.Add(this.butUpdate);
			this.groupGlobalUpdateFees.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupGlobalUpdateFees.Location = new System.Drawing.Point(368, 209);
			this.groupGlobalUpdateFees.Name = "groupGlobalUpdateFees";
			this.groupGlobalUpdateFees.Size = new System.Drawing.Size(248, 191);
			this.groupGlobalUpdateFees.TabIndex = 5;
			this.groupGlobalUpdateFees.TabStop = false;
			this.groupGlobalUpdateFees.Text = "Global Updates";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 114);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(208, 41);
			this.label9.TabIndex = 6;
			this.label9.Text = "Only for offices running reports on write-off estimates after updating fee schedu" +
    "les prior to selecting patients";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butUpdateWriteoffs
			// 
			this.butUpdateWriteoffs.Location = new System.Drawing.Point(6, 161);
			this.butUpdateWriteoffs.Name = "butUpdateWriteoffs";
			this.butUpdateWriteoffs.Size = new System.Drawing.Size(97, 24);
			this.butUpdateWriteoffs.TabIndex = 3;
			this.butUpdateWriteoffs.Text = "Update Write-offs";
			this.butUpdateWriteoffs.Click += new System.EventHandler(this.butUpdateWriteoffs_Click);
			// 
			// comboGlobalUpdateClinics
			// 
			this.comboGlobalUpdateClinics.IncludeAll = true;
			this.comboGlobalUpdateClinics.IncludeUnassigned = true;
			this.comboGlobalUpdateClinics.Location = new System.Drawing.Point(6, 26);
			this.comboGlobalUpdateClinics.Name = "comboGlobalUpdateClinics";
			this.comboGlobalUpdateClinics.SelectionModeMulti = true;
			this.comboGlobalUpdateClinics.Size = new System.Drawing.Size(231, 21);
			this.comboGlobalUpdateClinics.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(208, 18);
			this.label4.TabIndex = 5;
			this.label4.Text = "Update fees for all patients";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butUpdate
			// 
			this.butUpdate.Location = new System.Drawing.Point(6, 84);
			this.butUpdate.Name = "butUpdate";
			this.butUpdate.Size = new System.Drawing.Size(75, 24);
			this.butUpdate.TabIndex = 2;
			this.butUpdate.Text = "Update Fees";
			this.butUpdate.Click += new System.EventHandler(this.butUpdateFees_Click);
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.butImportCanada);
			this.groupBox5.Controls.Add(this.butImport);
			this.groupBox5.Controls.Add(this.butExport);
			this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox5.Location = new System.Drawing.Point(12, 302);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(214, 81);
			this.groupBox5.TabIndex = 3;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Export/Import";
			// 
			// butImportCanada
			// 
			this.butImportCanada.Location = new System.Drawing.Point(87, 51);
			this.butImportCanada.Name = "butImportCanada";
			this.butImportCanada.Size = new System.Drawing.Size(84, 24);
			this.butImportCanada.TabIndex = 2;
			this.butImportCanada.Text = "Import Canada";
			this.butImportCanada.Click += new System.EventHandler(this.butImportCanada_Click);
			// 
			// butImport
			// 
			this.butImport.Location = new System.Drawing.Point(87, 21);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(84, 24);
			this.butImport.TabIndex = 1;
			this.butImport.Text = "Import";
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// butExport
			// 
			this.butExport.Location = new System.Drawing.Point(6, 21);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(75, 24);
			this.butExport.TabIndex = 0;
			this.butExport.Text = "Export";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.groupBox4);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.butIncrease);
			this.groupBox3.Controls.Add(this.textPercent);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(368, 32);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(248, 167);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Increase by %";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(87, 140);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(90, 18);
			this.label5.TabIndex = 11;
			this.label5.Text = "(or decrease)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.radioPenny);
			this.groupBox4.Controls.Add(this.radioDime);
			this.groupBox4.Controls.Add(this.radioDollar);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(6, 49);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(208, 75);
			this.groupBox4.TabIndex = 1;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Round to nearest";
			// 
			// radioPenny
			// 
			this.radioPenny.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioPenny.Location = new System.Drawing.Point(14, 52);
			this.radioPenny.Name = "radioPenny";
			this.radioPenny.Size = new System.Drawing.Size(104, 17);
			this.radioPenny.TabIndex = 2;
			this.radioPenny.Text = "$.01";
			// 
			// radioDime
			// 
			this.radioDime.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioDime.Location = new System.Drawing.Point(14, 35);
			this.radioDime.Name = "radioDime";
			this.radioDime.Size = new System.Drawing.Size(104, 17);
			this.radioDime.TabIndex = 1;
			this.radioDime.Text = "$.10";
			// 
			// radioDollar
			// 
			this.radioDollar.Checked = true;
			this.radioDollar.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioDollar.Location = new System.Drawing.Point(14, 18);
			this.radioDollar.Name = "radioDollar";
			this.radioDollar.Size = new System.Drawing.Size(104, 17);
			this.radioDollar.TabIndex = 0;
			this.radioDollar.TabStop = true;
			this.radioDollar.Text = "$1";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(92, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(109, 18);
			this.label3.TabIndex = 6;
			this.label3.Text = "for example: 5";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butIncrease
			// 
			this.butIncrease.Location = new System.Drawing.Point(6, 137);
			this.butIncrease.Name = "butIncrease";
			this.butIncrease.Size = new System.Drawing.Size(75, 24);
			this.butIncrease.TabIndex = 2;
			this.butIncrease.Text = "Increase";
			this.butIncrease.Click += new System.EventHandler(this.butIncrease_Click);
			// 
			// textPercent
			// 
			this.textPercent.Location = new System.Drawing.Point(42, 23);
			this.textPercent.Name = "textPercent";
			this.textPercent.Size = new System.Drawing.Size(46, 20);
			this.textPercent.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(3, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 18);
			this.label2.TabIndex = 5;
			this.label2.Text = "%";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.butClear);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(12, 393);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(214, 79);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Clear";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(6, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(200, 30);
			this.label11.TabIndex = 7;
			this.label11.Text = "Clears all values from selected fee sched for selected prov and clinic";
			// 
			// butClear
			// 
			this.butClear.Location = new System.Drawing.Point(6, 49);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(75, 24);
			this.butClear.TabIndex = 0;
			this.butClear.Text = "Clear";
			this.butClear.Click += new System.EventHandler(this.butClear_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butPickGroupTo);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.comboGroupTo);
			this.groupBox1.Controls.Add(this.comboClinicTo);
			this.groupBox1.Controls.Add(this.comboProviderTo);
			this.groupBox1.Controls.Add(this.comboFeeSchedTo);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.butPickProvTo);
			this.groupBox1.Controls.Add(this.butPickSchedTo);
			this.groupBox1.Controls.Add(this.butCopy);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(12, 149);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(328, 141);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Copy To";
			// 
			// butPickGroupTo
			// 
			this.butPickGroupTo.Location = new System.Drawing.Point(279, 45);
			this.butPickGroupTo.Name = "butPickGroupTo";
			this.butPickGroupTo.Size = new System.Drawing.Size(23, 21);
			this.butPickGroupTo.TabIndex = 43;
			this.butPickGroupTo.Text = "...";
			this.butPickGroupTo.Visible = false;
			this.butPickGroupTo.Click += new System.EventHandler(this.butPickGroupTo_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(9, 47);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(90, 17);
			this.label6.TabIndex = 42;
			this.label6.Text = "Clinic";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboGroupTo
			// 
			this.comboGroupTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGroupTo.FormattingEnabled = true;
			this.comboGroupTo.Location = new System.Drawing.Point(-160, 47);
			this.comboGroupTo.Name = "comboGroupTo";
			this.comboGroupTo.Size = new System.Drawing.Size(174, 21);
			this.comboGroupTo.TabIndex = 42;
			this.comboGroupTo.Visible = false;
			// 
			// comboProviderTo
			// 
			this.comboProviderTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProviderTo.FormattingEnabled = true;
			this.comboProviderTo.Location = new System.Drawing.Point(100, 71);
			this.comboProviderTo.Name = "comboProviderTo";
			this.comboProviderTo.Size = new System.Drawing.Size(173, 21);
			this.comboProviderTo.TabIndex = 4;
			// 
			// comboFeeSchedTo
			// 
			this.comboFeeSchedTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSchedTo.Location = new System.Drawing.Point(100, 19);
			this.comboFeeSchedTo.Name = "comboFeeSchedTo";
			this.comboFeeSchedTo.Size = new System.Drawing.Size(173, 21);
			this.comboFeeSchedTo.TabIndex = 0;
			this.comboFeeSchedTo.SelectionChangeCommitted += new System.EventHandler(this.comboFeeCombos_SelectionChangeCommitted);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(26, 74);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(75, 16);
			this.label7.TabIndex = 39;
			this.label7.Text = "Provider";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickProvTo
			// 
			this.butPickProvTo.Location = new System.Drawing.Point(279, 71);
			this.butPickProvTo.Name = "butPickProvTo";
			this.butPickProvTo.Size = new System.Drawing.Size(23, 21);
			this.butPickProvTo.TabIndex = 5;
			this.butPickProvTo.Text = "...";
			this.butPickProvTo.Click += new System.EventHandler(this.butPickProvider_Click);
			// 
			// butPickSchedTo
			// 
			this.butPickSchedTo.Location = new System.Drawing.Point(279, 19);
			this.butPickSchedTo.Name = "butPickSchedTo";
			this.butPickSchedTo.Size = new System.Drawing.Size(23, 21);
			this.butPickSchedTo.TabIndex = 1;
			this.butPickSchedTo.Text = "...";
			this.butPickSchedTo.Click += new System.EventHandler(this.butPickFeeSched_Click);
			// 
			// butCopy
			// 
			this.butCopy.Location = new System.Drawing.Point(100, 106);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 24);
			this.butCopy.TabIndex = 6;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(18, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Fee Schedule";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(567, 455);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormFeeSchedTools2
			// 
			this.ClientSize = new System.Drawing.Size(654, 491);
			this.Controls.Add(this.checkShowGroups);
			this.Controls.Add(this.groupBox7);
			this.Controls.Add(this.groupGlobalUpdateFees);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormFeeSchedTools2";
			this.ShowInTaskbar = false;
			this.Text = "Fee Tools";
			this.Load += new System.EventHandler(this.FormFeeSchedTools_Load);
			this.groupBox7.ResumeLayout(false);
			this.groupGlobalUpdateFees.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormFeeSchedTools_Load(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.FeeSchedEdit)) {
				DialogResult=DialogResult.Cancel;
				Close();
				return;
			}
			//Only unrestricted users should be using fee tools with feeschedgroups.
			checkShowGroups.Visible=(PrefC.GetBool(PrefName.ShowFeeSchedGroups) && !Security.CurUser.ClinicIsRestricted);
			comboGroup.Location=new Point(100,46);
			comboGroupTo.Location=new Point(99,44);
			FillComboBoxes();
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				butImportCanada.Visible=false;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				groupGlobalUpdateFees.Enabled=false;
			}
		}

		private void FillComboBoxes() {
			long feeSchedNum1Selected=0;//Default to the first 
			if(comboFeeSched.SelectedIndex > -1) {
				feeSchedNum1Selected=_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum;
			}
			long feeSchedNum2Selected=0;//Default to the first
			if(comboFeeSchedTo.SelectedIndex > -1) {
				feeSchedNum2Selected=_listFeeScheds[comboFeeSchedTo.SelectedIndex].FeeSchedNum;
			}
			//The number of clinics and providers cannot change while inside this window.  Always reselect exactly what the user had before.
			int comboClinicIdx=comboClinic.SelectedIndex;
			int comboProvIdx=comboProvider.SelectedIndex;
			int comboProvToIdx=comboProviderTo.SelectedIndex;
			long feeSchedGroupNum=comboGroup.GetSelected<FeeSchedGroup>()?.FeeSchedGroupNum??0;
			long feeSchedGroupToNum=comboGroupTo.GetSelected<FeeSchedGroup>()?.FeeSchedGroupNum??0;
			comboFeeSched.Items.Clear();
			comboFeeSchedTo.Items.Clear();
			comboGroup.Items.Clear();
			comboGroupTo.Items.Clear();
			comboProvider.Items.Clear();
			comboProviderTo.Items.Clear();
			//Fee Schedules
			string str;
			for(int i=0;i<_listFeeScheds.Count;i++) {
				str=_listFeeScheds[i].Description;
				if(_listFeeScheds[i].FeeSchedType!=FeeScheduleType.Normal) {
					str+=" ("+_listFeeScheds[i].FeeSchedType.ToString()+")";
				}
				comboFeeSched.Items.Add(str);
				comboFeeSchedTo.Items.Add(str);
				if(_listFeeScheds[i].FeeSchedNum==feeSchedNum1Selected) {
					comboFeeSched.SelectedIndex=i;
				}
				if(_listFeeScheds[i].FeeSchedNum==feeSchedNum2Selected) {
					comboFeeSchedTo.SelectedIndex=i;
				}
			}
			if(_listFeeScheds.Count==0) {//No fee schedules in the database so set the first item to none.
				comboFeeSched.Items.Add(Lan.g(this,"None"));
				comboFeeSchedTo.Items.Add(Lan.g(this,"None"));
			}
			if(comboFeeSched.SelectedIndex==-1) {
				comboFeeSched.SelectedIndex=0;
			}
			if(comboFeeSchedTo.SelectedIndex==-1) {
				comboFeeSchedTo.SelectedIndex=0;
			}
			comboClinic.Items.Clear();
			if(PrefC.HasClinicsEnabled) {
				comboClinic.Items.Add(Lan.g(this,"Default"));
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Abbr);
				}
			}
			else {//No clinics
				//Add none even though clinics is turned off so that 0 is a valid index to select.
				comboClinic.Items.Add(Lan.g(this,"None"));
				//For UI reasons, leave the clinic combo boxes visible for users not using clinics and they will just say "none".
				comboClinic.Enabled=false;
				butPickClinic.Enabled=false;
			}
			//Fee Sched Groups
			if(checkShowGroups.Visible) {//Always run the fill logic if they are using the groups feaure, not just if the combobox is showing.
				FillFeeSchedGroupComboBox(comboGroup,_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum,feeSchedGroupNum);
				FillFeeSchedGroupComboBox(comboGroupTo,_listFeeScheds[comboFeeSchedTo.SelectedIndex].FeeSchedNum,feeSchedGroupToNum);
			}
			//Providers
			comboProvider.Items.Add(Lan.g(this,"None"));
			comboProviderTo.Items.Add(Lan.g(this,"None"));
			for(int i=0;i<_listProvs.Count;i++) {
				comboProvider.Items.Add(_listProvs[i].Abbr);
				comboProviderTo.Items.Add(_listProvs[i].Abbr);
			}
			comboClinic.SelectedIndex=comboClinicIdx > -1 ? comboClinicIdx:0;
			comboProvider.SelectedIndex=comboProvIdx > -1 ? comboProvIdx:0;
			comboProviderTo.SelectedIndex=comboProvToIdx > -1 ? comboProvToIdx:0;
			//Global----------------------------------------------------------------------------------------------------
			if(_listFeeScheds[comboFeeSched.SelectedIndex].IsGlobal) {
				comboClinic.Enabled=false;
				butPickClinic.Enabled=false;
				comboClinic.SelectedIndex=0;				
				comboProvider.Enabled=false;
				butPickProv.Enabled=false;
				comboProvider.SelectedIndex=0;
				comboGroup.Enabled=false;
				comboGroup.SelectedIndex=-1;
			}
			else {
				if(PrefC.HasClinicsEnabled) {
					comboClinic.Enabled=true;
					butPickClinic.Enabled=true;
					comboGroup.Enabled=true;
				}
				comboProvider.Enabled=true;
				butPickProv.Enabled=true;
			}
			if(_listFeeScheds[comboFeeSchedTo.SelectedIndex].IsGlobal) {
				comboClinicTo.Enabled=false;
				comboClinicTo.IsUnassignedSelected=true;
				comboProviderTo.Enabled=false;
				butPickProvTo.Enabled=false;
				butPickGroupTo.Enabled=false;
				comboProviderTo.SelectedIndex=0;
				comboGroupTo.Enabled=false;
				comboGroupTo.SelectedIndex=-1;
			}
			else {
				if(PrefC.HasClinicsEnabled) {
					comboClinicTo.Enabled=true;
					comboGroupTo.Enabled=true;
					butPickGroupTo.Enabled=true;
				}
				comboProviderTo.Enabled=true;
				butPickProvTo.Enabled=true;
			}
		}

		private void FillFeeSchedGroupComboBox(ComboBox comboFeeSchedGroup,long feeSchedNumSelected,long feeSchedGroupNum) {
			List<long> listComboGroupNums=new List<long>();
			List<FeeSchedGroup> listGroups=FeeSchedGroups.GetAllForFeeSched(feeSchedNumSelected);
			foreach(FeeSchedGroup feeSchedGroupCur in listGroups) {
				if(!listComboGroupNums.Contains(feeSchedGroupCur.FeeSchedGroupNum) && feeSchedGroupCur.ListClinicNumsAll.Count>0) {//Skip duplicate/empty groups.
					ODBoxItem<FeeSchedGroup> boxItemFeeSchedGroup=new ODBoxItem<FeeSchedGroup>(feeSchedGroupCur.Description,feeSchedGroupCur);
					comboFeeSchedGroup.Items.Add(boxItemFeeSchedGroup);
					listComboGroupNums.Add(feeSchedGroupCur.FeeSchedGroupNum);
					//Set this fee sched group as the selection if it matches the previous selection before refreshing
					if(boxItemFeeSchedGroup.Tag.FeeSchedGroupNum==feeSchedGroupNum) {
						comboFeeSchedGroup.SelectedItem=boxItemFeeSchedGroup;
					}
				}
			}
		}

		private void butClear_Click(object sender, System.EventArgs e) {
			if(PrefC.HasClinicsEnabled) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will clear all values from the selected fee schedule for the currently selected clinic and provider.  Are you sure you want to continue?")) {
					return;
				}
			}
			else if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will clear all values from the selected fee schedule for the currently selected provider.  Are you sure you want to continue?")) {
				return;
			}
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex!=0){
				listClinicNums.Add(_listClinics[comboClinic.SelectedIndex-1].ClinicNum);
			}
			long feeSchedNum=_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum;
			if(PrefC.GetBool(PrefName.ShowFeeSchedGroups) && !checkShowGroups.Checked) {
				FeeSchedGroup groupCur=FeeSchedGroups.GetOneForFeeSchedAndClinic(feeSchedNum,_listClinics[comboClinic.SelectedIndex-1].ClinicNum);
				if(groupCur!=null) {
					MsgBox.Show(Lans.g(this,"Selected clinic is a member of Fee Schedule Group: ")+groupCur.Description
						+Lans.g(this," and must be cleared at the group level."));
					return;
				}
			}
			if(checkShowGroups.Checked) {
				if(comboGroup.SelectedIndex==-1) {
					MsgBox.Show(this,"Please select a Fee Schedule Group.");
					return;
				}
				//Fees.ImportFees() will update the rest of the group.
				listClinicNums.AddRange(comboGroup.GetSelected<FeeSchedGroup>().ListClinicNumsAll);
			}
			if(listClinicNums.IsNullOrEmpty()) {
				listClinicNums.Add(0);
			}
			long provNum=0;
			if(comboProvider.SelectedIndex!=0) {
				provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			//ODProgress.ShowAction(() => {
			string logText="";
			foreach(long clinicNum in listClinicNums) {
				Fees.DeleteFees(feeSchedNum,clinicNum,provNum);
				logText+=Lan.g(this,"Procedures for Fee Schedule")+" "+FeeScheds.GetDescription(feeSchedNum);
				if(PrefC.HasClinicsEnabled) {
					if(Clinics.GetAbbr(Clinics.ClinicNum)=="") {
						logText+=Lan.g(this," at Headquarters");
					}
					else {
						logText+=Lan.g(this," at clinic")+" "+Clinics.GetAbbr(Clinics.ClinicNum);
					}
				}
				logText+=" "+Lan.g(this,"were all cleared.")+"\r\n";
			}
			SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,logText);
			//	});
			MsgBox.Show(this,"Done");
		}

		private void butCopy_Click(object sender, System.EventArgs e) {
			List<long> listClinicNumsTo=comboClinicTo.ListSelectedClinicNums;
			if(checkShowGroups.Checked) {
				if(comboGroup.SelectedIndex<0 || comboGroupTo.SelectedIndex<0) {
					MsgBox.Show(this,"Please select a Fee Schedule group.");
					return;
				}
				listClinicNumsTo=comboGroupTo.GetSelected<FeeSchedGroup>().ListClinicNumsAll;
			}
			if(PrefC.HasClinicsEnabled && listClinicNumsTo.Count==0) {
				MsgBox.Show(this,"At least one \"Clinic To\" clinic must be selected.");
				return;
			}
			long toProvNum=0;
			if(comboProviderTo.SelectedIndex!=0) {
				toProvNum=_listProvs[comboProviderTo.SelectedIndex-1].ProvNum;
			}
			FeeSched toFeeSched=_listFeeScheds[comboFeeSchedTo.SelectedIndex];
			long fromClinicNum=0;
			if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex!=0){
				fromClinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			long fromProvNum=0;
			if(comboProvider.SelectedIndex!=0) {
				fromProvNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			if(checkShowGroups.Checked) {
				//verify we aren't copying the same group into itself
				if(comboGroup.GetSelected<FeeSchedGroup>().FeeSchedGroupNum==comboGroupTo.GetSelected<FeeSchedGroup>().FeeSchedGroupNum && fromProvNum==toProvNum) {
					MsgBox.Show(this,"Fee Schedule Groups are not allowed to be copied into themselves. Please choose another fee schedule group to copy.");
					return;
				}
				//Get fromclinicnum from list of group clinics.
				fromClinicNum=comboGroup.GetSelected<FeeSchedGroup>().ListClinicNumsAll.FirstOrDefault();
			}
			FeeSched fromFeeSched=_listFeeScheds[comboFeeSched.SelectedIndex];
			if(fromFeeSched.FeeSchedNum==toFeeSched.FeeSchedNum
				&& fromProvNum==toProvNum
				&& (!PrefC.HasClinicsEnabled || fromClinicNum.In(listClinicNumsTo)))//If clinics disabled, can cause false negative so shortcircuit
			{
				MsgBox.Show(this,"Fee Schedules are not allowed to be copied into themselves. Please choose another fee schedule to copy.");
				return;
			}
			if(PrefC.GetBool(PrefName.ShowFeeSchedGroups) && !checkShowGroups.Checked) {
				//Pref is on but we are copying clinics.
				foreach(long clinicNumTo in listClinicNumsTo) {
					FeeSchedGroup groupCur=FeeSchedGroups.GetOneForFeeSchedAndClinic(toFeeSched.FeeSchedNum,clinicNumTo);
					if(groupCur!=null) {
						Clinic clinicCur=Clinics.GetClinic(clinicNumTo);
						MsgBox.Show(this,Lans.g(this,"Clinic: ")+clinicCur.Abbr+Lans.g(this," is a member of Fee Schedule Group: ")+groupCur.Description
							+Lans.g(this," for the selected Fee Schedule and must be copied at the Fee Schedule Group level."));
						return;
					}
				}
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"All fees that exactly match the \"Copy To\" fee schedule/clinic/provider combination will be deleted.  Then new fees will be copied in.  Are you sure you want to continue?")){
				return;
			}
			ODProgress.ShowAction(() => FeeScheds.CopyFeeSchedule(fromFeeSched,fromClinicNum,fromProvNum,toFeeSched,listClinicNumsTo,toProvNum),
					startingMessage:"Preparing to copy fees...",
					progStyle:ProgressBarStyle.Continuous,
					eventType:typeof(FeeSchedEvent),
					odEventType:ODEventType.FeeSched);
			//After finishing, clear the Copy To section, but leave the Copy From section as is.
			comboFeeSchedTo.SelectedIndex=0;
			comboClinicTo.IsNothingSelected=true;
			comboProviderTo.SelectedIndex=0;
			MsgBox.Show(this,"Done.");
		}

		private void butIncrease_Click(object sender, System.EventArgs e) {
			int percent=0;
			if(textPercent.Text==""){
				MsgBox.Show(this,"Please enter a percent first.");
				return;
			}
			try{
				percent=System.Convert.ToInt32(textPercent.Text);
			}
			catch{
				MsgBox.Show(this,"Percent is not a valid number.");
				return;
			}
			if(percent<-99 || percent>99){
				MsgBox.Show(this,"Percent must be between -99 and 99.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will overwrite all values of the selected fee schedule/clinic/provider combo.  Previously entered fee "
				+"amounts will be lost.  It is recommended to first create a backup copy of the original fee schedule, then update the original fee schedule "
				+"with the new fees.  Are you sure you want to continue?"))
			{
				return;
			}
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex>0){
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			long feeSchedNum=_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum;
			if(PrefC.GetBool(PrefName.ShowFeeSchedGroups) && !checkShowGroups.Checked) {
				FeeSchedGroup groupCur=FeeSchedGroups.GetOneForFeeSchedAndClinic(feeSchedNum,clinicNum);
				if(groupCur!=null) {
					MsgBox.Show(Lans.g(this,"Selected clinic is a member of Fee Schedule Group: ")+groupCur.Description
						+Lans.g(this," and must be increased at the group level."));
					return;
				}
			}
			if(checkShowGroups.Checked) {
				if(comboGroup.SelectedIndex==-1) {
					MsgBox.Show(this,"Please select a Fee Schedule Group.");
					return;
				}
				//Fees.ImportFees() will update the rest of the group.
				clinicNum=comboGroup.GetSelected<FeeSchedGroup>().ListClinicNumsAll.FirstOrDefault();
			}
			long provNum=0;
			if(comboProvider.SelectedIndex>0){
				provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			List<Fee> listFees=Fees.GetListExact(feeSchedNum,clinicNum,provNum);
			bool doIncreaseFees=EvaluateOverrides(clinicNum,provNum,feeSchedNum,listFees);
			if(!doIncreaseFees) {
				return;//either no fees would be updated or the user chose to cancel and review so don't increase fees.
			}
			int round=0;//Default to dollar
			if(radioDime.Checked){
				round=1;
			}
			if(radioPenny.Checked){
				round=2;
			}
			ODProgress.ShowAction(() => {
					listFees=Fees.IncreaseNew(feeSchedNum,percent,round,listFees,clinicNum,provNum);
					string procCode;
					for(int i=0;i<listFees.Count;i++) {
						if(listFees[i].Amount==0) {
							continue;
						}
						try {
							procCode=ProcedureCodes.GetStringProcCode(listFees[i].CodeNum);
						}
						catch(Exception) {//if CodeNum is not in the procedurecode table, don't make securitylog entry
							continue;
						}
						Fees.Update(listFees[i]);//only a few hundred calls, max
						string logText="Procedure: "+procCode+", "
							+"Fee: "+listFees[i].Amount.ToString("c")+", "
							+"Fee Schedule: "+FeeScheds.GetDescription(listFees[i].FeeSched);
						if(PrefC.HasClinicsEnabled) {
							if(Clinics.GetAbbr(clinicNum)=="") {
								logText+=", at Headquarters";
							}
							else {
								logText+=", at clinic: "+Clinics.GetAbbr(clinicNum);
							}
						}
						if(provNum!=0) {
							logText+=", for provider: "+Providers.GetAbbr(provNum);
						}
						logText+=". Fee increased by "+((float)percent/100.0f).ToString("p")+" using the increase "
							+"button in the Fee Tools window.";
						SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,logText,listFees[i].CodeNum,DateTime.MinValue);
						SecurityLogs.MakeLogEntry(Permissions.LogFeeEdit,0,"Fee Updated",listFees[i].FeeNum,listFees[i].SecDateTEdit);
						FeeSchedEvent.Fire(ODEventType.FeeSched,"Modifying fees, please wait...");
					}
				},
				startingMessage:Lan.g(this,"Preparing to modify fees")+"...",
				eventType:typeof(FeeSchedEvent),
				odEventType:ODEventType.FeeSched);
			MsgBox.Show(this,"Done.");
		}

		///<summary>Determines if there are overrides being updated, or just a regular feeSchedule. Returns true if there are overrides and user wants to 
		///continue, or if it is a regular fee schedule. Returns false is it is an override schedule and there are no overrides to update or if user 
		///chooses to cancel to review.</summary>
		private bool EvaluateOverrides(long clinicNum,long provNum,long feeSchedNum,List<Fee> listFees) {
			//listFees only includes exact matches 
			int countTotalFeesForSched=Fees.GetCountByFeeSchedNum(feeSchedNum);
			string msgText="";
			string clinicName=Clinics.GetAbbr(clinicNum);
			string provName=Providers.GetAbbr(provNum);
			string feeSchedDesc=_listFeeScheds.FirstOrDefault(x => x.FeeSchedNum==feeSchedNum).Description;
			if(clinicNum==0 && provNum==0){
				return true;
			}
			if(clinicNum!=0 && provNum!=0) {//user seems to be trying to increase fee overrides for clinic and prov
				if(listFees.Count==0) {//but there aren't any.
					msgText="There are no overrides for clinic '"+clinicName+"' and provider '"+provName+"' "
						+"so no fees will be updated. If you want to create overrides, first enter or copy fees into that override fee schedule.";
					MessageBox.Show(msgText);
					return false;//don't run increase tool
				}
				if(listFees.Count!=countTotalFeesForSched) {//
					msgText="There are "+listFees.Count+" override fees for clinic '"+clinicName+"' and provider"
						+" '"+provName+"' and there are "+countTotalFeesForSched+" total fees for fee schedule '"
						+feeSchedDesc+"'. Only the "+listFees.Count+" fees will be increased.  Cancel if you want to review first.";
					if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
						return false;
					}
				}
				return true;
			}
			else if(clinicNum!=0) {
				if(listFees.Count==0) {
					msgText="There are no overrides for clinic '"+clinicName+"' "
						+"so no fees will be updated. If you want to create overrides, first enter or copy fees into that override fee schedule.";
					MessageBox.Show(msgText);
					return false;
				}
				if(listFees.Count!=countTotalFeesForSched) {
					if(checkShowGroups.Checked){
						//We know at this point we already selected a valid feeschedgroup and don't need a null check.
						FeeSchedGroup groupCur=FeeSchedGroups.GetOneForFeeSchedAndClinic(feeSchedNum,clinicNum);
						msgText="There are "+listFees.Count+" override fees for group '"+groupCur.Description+"' and there are"
							+" "+countTotalFeesForSched+" total fees for fee schedule '"+feeSchedDesc+"'. Only the "
							+listFees.Count+" fees will be increased. Cancel if you want to review first.";
						if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
							return false;
						}
					}
					else {
						msgText="There are "+listFees.Count+" override fees for clinic '"+clinicName+"' and there are"
							+" "+countTotalFeesForSched+" total fees for fee schedule '"+feeSchedDesc+"'. Only the "
							+listFees.Count+" fees will be increased. Cancel if you want to review first.";
						if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
							return false;
						}
					}
				}
				return true;	
			}
			else if(provNum!=0) {
				if(listFees.Count==0) {
					msgText="There are no overrides for provider '"+provName+"' "
						+"so no fees will be updated. If you want to create overrides, first enter or copy fees into that override fee schedule.";
					MessageBox.Show(msgText);
					return false;
				}
				if(listFees.Count!=countTotalFeesForSched) {
					msgText="There are "+listFees.Count+" override fees for provider '"+provName+"' and there are"
						+" "+countTotalFeesForSched+" total fees for fee schedule '"+feeSchedDesc+"'. Only the "
						+listFees.Count+" fees will be increased. Cancel if you want to review first.";
					if(MessageBox.Show(msgText,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
						return false;
					}
				}
				return true;
			}
			return true;//will never hit
		}

		private void butExport_Click(object sender,EventArgs e) {
			long feeSchedNum=_listFeeScheds[comboFeeSched.SelectedIndex].FeeSchedNum;
			string feeSchedDesc=FeeScheds.GetDescription(feeSchedNum);
			//scrub out any non-AlphaNumeric characters.
			feeSchedDesc=Regex.Replace(feeSchedDesc,"(?:[^a-z0-9 ]|(?<=['\"])s)","",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
			FeeSched feeSched=_listFeeScheds[comboFeeSched.SelectedIndex];
			long clinicNum=0;
			if(comboClinic.SelectedIndex!=0) {
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			if(checkShowGroups.Checked) {
				if(comboGroup.SelectedIndex==-1) {
					MsgBox.Show(this,"Please select a Fee Schedule Group.");
					return;
				}
				clinicNum=comboGroup.GetSelected<FeeSchedGroup>().ListClinicNumsAll.FirstOrDefault();
			}
			long provNum=0;
			if(comboProvider.SelectedIndex!=0) {
				provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			string fileName="Fees"+feeSchedDesc+".txt";
			string filePath=ODFileUtils.CombinePaths(Path.GetTempPath(),fileName);
			if(ODBuild.IsWeb()) {
				//file download dialog will come up later, after file is created.
			}
			else {
				Cursor=Cursors.WaitCursor;
				SaveFileDialog Dlg=new SaveFileDialog();
				if(Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
					Dlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
				}
				else if(Directory.Exists("C:\\")) {
					Dlg.InitialDirectory="C:\\";
				}
				Dlg.FileName=fileName;
				if(Dlg.ShowDialog()!=DialogResult.OK) {
					Cursor=Cursors.Default;
					return;
				}
				filePath=Dlg.FileName;
			}
			ODProgress.ShowAction(
				() => {
					FeeScheds.ExportFeeSchedule(feeSched.FeeSchedNum,clinicNum,provNum,filePath);
				},
				startingMessage:Lan.g(this,"Preparing to export fees")+"...",
				progStyle:ProgressBarStyle.Continuous,
				eventType:typeof(FeeSchedEvent),
				odEventType:ODEventType.FeeSched);
			if(ODBuild.IsWeb()) {
				ThinfinityUtils.ExportForDownload(filePath);
			}
			else {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Fee schedule exported.");
			}
		}

		private void butImport_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"If you want a clean slate, you should clear the current fee schedule first.  When imported, any fees that are found in the text file will overwrite values of the selected fee schedule/clinic/provider combo.  Are you sure you want to continue?")) 
			{
				return;
			}
			OpenFileDialog Dlg=new OpenFileDialog();
			if(Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
				Dlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			else if(Directory.Exists("C:\\")) {
				Dlg.InitialDirectory="C:\\";
			}
			if(Dlg.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(!File.Exists(Dlg.FileName)){
				MsgBox.Show(this,"File not found");
				return;
			}
			Cursor=Cursors.WaitCursor;
			//Import deletes fee if it exists and inserts new fees based on fee settings.
			long clinicNum=0;
			if(comboClinic.SelectedIndex!=0) {
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			FeeSched feeSched=_listFeeScheds[comboFeeSched.SelectedIndex];
			if(PrefC.GetBool(PrefName.ShowFeeSchedGroups) && !checkShowGroups.Checked) {
				FeeSchedGroup groupCur=FeeSchedGroups.GetOneForFeeSchedAndClinic(feeSched.FeeSchedNum,clinicNum);
				if(groupCur!=null) {
					MsgBox.Show(Lans.g(this,"Selected clinic is a member of Fee Schedule Group: ")+groupCur.Description
						+Lans.g(this," and must be imported at the group level."));
					return;
				}
			}
			if(checkShowGroups.Checked) {
				if(comboGroup.SelectedIndex==-1) {
					MsgBox.Show(this,"Please select a Fee Schedule Group.");
					return;
				}
				//Fees.ImportFees() will update the rest of the group.
				clinicNum=comboGroup.GetSelected<FeeSchedGroup>().ListClinicNumsAll.FirstOrDefault();
			}
			long provNum=0;
			if(comboProvider.SelectedIndex!=0) {
				provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			bool isImportSuccessful=true;
			ODProgress.ShowAction(
				() => {
					try {
						FeeL.ImportFees(Dlg.FileName,feeSched.FeeSchedNum,clinicNum,provNum);
					}
					catch(Exception ex) {
						FriendlyException.Show("Error importing fees.",ex);
						isImportSuccessful=false;
					}
				},
				startingMessage:"Importing fees...",
				progStyle:ProgressBarStyle.Continuous,
				eventType:typeof(FeeSchedEvent),
				odEventType:ODEventType.FeeSched);
			//Progress bar won't go away.  No big deal I guess.
			Cursor=Cursors.Default;
			if(isImportSuccessful) { 
				MsgBox.Show(this,"Fee schedule imported.");
			}
		}

		private void butImportCanada_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"If you want a clean slate, the current fee schedule should be cleared first.  When imported, any fees that are found in the text file will overwrite values of the current fee schedule showing in the main window.  Are you sure you want to continue?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			FormFeeSchedPickRemote formPick=new FormFeeSchedPickRemote();
			formPick.Url=@"http://www.opendental.com/feescanada/";//points to index.php file
			if(formPick.ShowDialog()!=DialogResult.OK) {
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.WaitCursor;//original wait cursor seems to go away for some reason.
			Application.DoEvents();
			string feeData="";
			Action actionCloseFeeSchedImportCanadaProgress=ODProgress.Show(ODEventType.FeeSched,typeof(FeeSchedEvent));
			if(formPick.IsFileChosenProtected) {
				actionCloseFeeSchedImportCanadaProgress?.Invoke();//Hide the progress window so it does not cover up the authorization form.
				string memberNumberODA="";
				string memberPasswordODA="";
				if(formPick.FileChosenName.StartsWith("ON_")) {//Any and all Ontario fee schedules
					FormFeeSchedPickAuthOntario formAuth=new FormFeeSchedPickAuthOntario();
					if(formAuth.ShowDialog()!=DialogResult.OK) {
						Cursor=Cursors.Default;
						return;
					}
					actionCloseFeeSchedImportCanadaProgress=ODProgress.Show(ODEventType.FeeSched,typeof(FeeSchedEvent));
					memberNumberODA=formAuth.ODAMemberNumber;
					memberPasswordODA=formAuth.ODAMemberPassword;
				}
				//prepare the xml document to send--------------------------------------------------------------------------------------
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = ("    ");
				StringBuilder strbuild=new StringBuilder();
				using(XmlWriter writer=XmlWriter.Create(strbuild,settings)) {
					writer.WriteStartElement("RequestFeeSched");
					writer.WriteStartElement("RegistrationKey");
					writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));
					writer.WriteEndElement();//RegistrationKey
					writer.WriteStartElement("FeeSchedFileName");
					writer.WriteString(formPick.FileChosenName);
					writer.WriteEndElement();//FeeSchedFileName
					if(memberNumberODA!="") {
						writer.WriteStartElement("ODAMemberNumber");
						writer.WriteString(memberNumberODA);
						writer.WriteEndElement();//ODAMemberNumber
						writer.WriteStartElement("ODAMemberPassword");
						writer.WriteString(memberPasswordODA);
						writer.WriteEndElement();//ODAMemberPassword
					}
					writer.WriteEndElement();//RequestFeeSched
				}
#if DEBUG
				OpenDental.localhost.Service1 updateService=new OpenDental.localhost.Service1();
#else
				OpenDental.customerUpdates.Service1 updateService=new OpenDental.customerUpdates.Service1();
				updateService.Url=PrefC.GetString(PrefName.UpdateServerAddress);
#endif
				//Send the message and get the result-------------------------------------------------------------------------------------
				string result="";
				try {
					FeeSchedEvent.Fire(ODEventType.FeeSched,Lan.g(this,"Retrieving fee schedule")+"...");
					result=updateService.RequestFeeSched(strbuild.ToString());
				}
				catch(Exception ex) {
					actionCloseFeeSchedImportCanadaProgress?.Invoke();
					Cursor=Cursors.Default;
					MessageBox.Show("Error: "+ex.Message);
					return;
				}
				Cursor=Cursors.Default;
				XmlDocument doc=new XmlDocument();
				doc.LoadXml(result);
				//Process errors------------------------------------------------------------------------------------------------------------
				XmlNode node=doc.SelectSingleNode("//Error");
				if(node!=null) {
					actionCloseFeeSchedImportCanadaProgress?.Invoke();
					MessageBox.Show(node.InnerText,"Error");
					return;
				}
				node=doc.SelectSingleNode("//KeyDisabled");
				if(node==null) {
					//no error, and no disabled message
					if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,false)) {//this is one of three places in the program where this happens.
						DataValid.SetInvalid(InvalidType.Prefs);
					}
				}
				else {
					actionCloseFeeSchedImportCanadaProgress?.Invoke();
					MessageBox.Show(node.InnerText);
					if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,true)) {//this is one of three places in the program where this happens.
						DataValid.SetInvalid(InvalidType.Prefs);
					}
					return;
				}
				//Process a valid return value------------------------------------------------------------------------------------------------
				node=doc.SelectSingleNode("//ResultCSV64");
				string feeData64=node.InnerXml;
				byte[] feeDataBytes=Convert.FromBase64String(feeData64);
				feeData=Encoding.UTF8.GetString(feeDataBytes);
			}
			else {
				FeeSchedEvent.Fire(ODEventType.FeeSched,Lan.g(this,"Downloading fee schedule")+"...");
				string tempFile=PrefC.GetRandomTempFile(".tmp");
				WebClient myWebClient=new WebClient();
				try {
					myWebClient.DownloadFile(formPick.FileChosenUrl,tempFile);
				}
				catch(Exception ex) {
					actionCloseFeeSchedImportCanadaProgress?.Invoke();
					MessageBox.Show(Lan.g(this,"Failed to download fee schedule file")+": "+ex.Message);
					Cursor=Cursors.Default;
					return;
				}
				feeData=File.ReadAllText(tempFile);
				File.Delete(tempFile);
			}
			int numImported;
			int numSkipped;
			long clinicNum=0;
			if(comboClinic.SelectedIndex!=0) {
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			FeeSched feeSched=_listFeeScheds[comboFeeSched.SelectedIndex];
			if(PrefC.GetBool(PrefName.ShowFeeSchedGroups) && !checkShowGroups.Checked) {
				FeeSchedGroup groupCur=FeeSchedGroups.GetOneForFeeSchedAndClinic(feeSched.FeeSchedNum,clinicNum);
				if(groupCur!=null) {
					MsgBox.Show(Lans.g(this,"Selected clinic is a member of Fee Schedule Group: ")+groupCur.Description
						+Lans.g(this," and must be imported at the group level."));
					return;
				}
			}
			if(checkShowGroups.Checked) {
				if(comboGroup.SelectedIndex==-1) {
					MsgBox.Show(this,"Please select a Fee Schedule Group.");
					return;
				}
				//Fees.ImportFees() will update the rest of the group.
				clinicNum=comboGroup.GetSelected<FeeSchedGroup>().ListClinicNumsAll.FirstOrDefault();
			}
			long provNum=0;
			if(comboProvider.SelectedIndex!=0) {
				provNum=_listProvs[comboProvider.SelectedIndex-1].ProvNum;
			}
			FeeScheds.ImportCanadaFeeSchedule2(feeSched,feeData,clinicNum,provNum,out numImported,out numSkipped);
			actionCloseFeeSchedImportCanadaProgress?.Invoke();
			Cursor=Cursors.Default;
			DialogResult=DialogResult.OK;
			string outputMessage="Done. Number imported: "+numImported;
			if(numSkipped>0) {
				outputMessage+=" Number skipped: "+numSkipped;
			}
			MessageBox.Show(outputMessage);
		}

		private void butUpdateFees_Click(object sender,EventArgs e) {
			long rowsChanged=0;
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"All treatment planned procedures for all patients will be updated.  Only the fee will be updated, not the insurance "
				+"estimate.  It might take a few minutes.  Continue?")) {
				return;
			}
			ODProgressExtended progExtended=new ODProgressExtended(ODEventType.FeeSched,new FeeSchedEvent(),this,
				tag:new ProgressBarHelper("Fee Schedule Update Progress",progressBarEventType:ProgBarEventType.Header),cancelButtonText:Lan.g(this,"Close"));
			Cursor=Cursors.WaitCursor;
			List<Fee> listFeesHQ=Fees.GetByClinicNum(0);//All HQ fees
			try {
				if(PrefC.HasClinicsEnabled) {
					List<long> listFeeClinics=comboGlobalUpdateClinics.ListSelectedClinicNums;
					for(int i=0;i<listFeeClinics.Count;i++) {
						//Clinic clinicCur=listFeeClinics[i];
						while(progExtended.IsPaused) {
							Thread.Sleep(10);
							if(progExtended.IsCanceled) {
								break;
							}
						}
						if(progExtended.IsCanceled) {
							break;
						}
						double percentComplete=(((double)i)/listFeeClinics.Count*100);
						if(listFeeClinics.Count>1) {
							progExtended.Fire(ODEventType.FeeSched,new ProgressBarHelper("Overall",(int)percentComplete+"%",i,
								listFeeClinics.Count,tagString:"OverallStatus"));
							progExtended.Fire(ODEventType.FeeSched,new ProgressBarHelper(Clinics.GetAbbr(listFeeClinics[i]),"0%",1,100,tagString:"Clinic"));
						}
						else {
							progExtended.Fire(ODEventType.FeeSched,new ProgressBarHelper(Clinics.GetAbbr(listFeeClinics[i]),"0%",1,100,tagString:"Clinic"));
							progExtended.HideButtons();//can't pause or cancel with 1 clinic. This event needs to be called after the bar is instantiated. 
						}
						rowsChanged+=Procedures.GlobalUpdateFees(listFeesHQ,listFeeClinics[i],Clinics.GetAbbr(listFeeClinics[i]));
						if(progExtended.IsPaused) {
							progExtended.AllowResume();
						}
					}
					if(listFeeClinics.Count>1) {
						progExtended.Fire(ODEventType.FeeSched,new ProgressBarHelper("Overall","100%",100,100,tagString:"OverallStatus"));
					}
				}
				else {//no clinic - "Clinic" here is just a reference to the progress bar that updates Clinic progress instead of overall progress
					progExtended.Fire(ODEventType.FeeSched,new ProgressBarHelper("Updating...","0%",1,100,tagString:"Clinic"));
					progExtended.HideButtons();
					rowsChanged=Procedures.GlobalUpdateFees(listFeesHQ,0,"Updating...");
				}
				progExtended.OnProgressDone();
				progExtended.Fire(ODEventType.FeeSched,new ProgressBarHelper("Treatment planned procedure fees changed: "+rowsChanged.ToString()+"\r\nDone.",
					progressBarEventType:ProgBarEventType.TextMsg));
			}
			catch(ApplicationException ex) {
				Cursor=Cursors.Default;
				progExtended.Close();
				MessageBox.Show(ex.Message);
				return;	
			}
			finally {
				if(progExtended.IsCanceled) {//close
					progExtended.Close();
					DialogResult=DialogResult.OK;
				}
			}
			Cursor=Cursors.Default;
		}

		private void butUpdateWriteoffs_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Write-off estimates will be recalculated for all treatment planned procedures.  This tool should only "
				+"be run if you have updated fee schedules and want to run reports on write-off estimates for patients that have not been viewed."
				+"\r\n\r\nThis could take a very long time.  Continue?"))
			{
				return;
			}
			List<long> listWriteoffClinics=comboGlobalUpdateClinics.ListSelectedClinicNums;
			bool doUpdatePrevClinicPref=comboGlobalUpdateClinics.IsAllSelected;//keeps track of current ClinicNum in db to resume if interrupted
			//MUST be in primary key order so that we will resume on the correct clinic and update the remaining clinics in the list
			if(doUpdatePrevClinicPref){
				listWriteoffClinics.Sort(); 
			}
			if(listWriteoffClinics.Count==0) {//This is assuming clinics are turned off
				listWriteoffClinics.Add(0);
			}
			int indexPrevClinic=-1;
			if(PrefC.HasClinicsEnabled
				&& !Security.CurUser.ClinicIsRestricted
				&& comboGlobalUpdateClinics.IsAllSelected 
				&& !string.IsNullOrEmpty(PrefC.GetString(PrefName.GlobalUpdateWriteOffLastClinicCompleted)))//previous 'All' run was interrupted, resume
			{
				try {
					long prevClinicNum=PrefC.GetLong(PrefName.GlobalUpdateWriteOffLastClinicCompleted);
					indexPrevClinic=listWriteoffClinics.FindIndex(x => x==prevClinicNum);
				}
				catch(Exception ex) {
					ex.DoNothing();//if pref is not a long, leave prevClinic as -1 so it will run as if it was not previously interrupted
				}
			}
			if(indexPrevClinic>-1 //only true if clinics are enabled, the user is not restricted, updating all clinics, and the pref has been set from previous run
				&& listWriteoffClinics.Count>indexPrevClinic+1) //we will skip indexPrevClinic+1 items and there needs to be at least one more clinic to process
			{
				string msgText=Lan.g(this,"This tool was paused or interrupted during a previous run.  Would you like to resume the previous run?")+"\r\n\r\n"
					+Lan.g(this,"Yes - Run the tool beginning where the previous run left off.")+"\r\n\r\n"
					+Lan.g(this,"No - Run the tool for all clinics and replace the previous run progress with the progress of this run.")+"\r\n\r\n"
					+Lan.g(this,"Cancel - Don't run the tool and retain the previous run progress.");
				DialogResult diagRes=MessageBox.Show(this,msgText,"",MessageBoxButtons.YesNoCancel);
				if(diagRes==DialogResult.Cancel) {
					return;
				}
				else if(diagRes==DialogResult.Yes) {//pick up where last run left off and overwrite that last clinic with the progress from this run
					listWriteoffClinics.RemoveRange(0,indexPrevClinic+1);
				}
				else {
					//diagRes==DialogResult.No, run tool for all clinics and replace the previous run progress with the progress from this run
				}
			}
			ODProgressExtended progress=new ODProgressExtended(ODEventType.FeeSched,new FeeSchedEvent(),this,
				tag:new ProgressBarHelper(Lan.g(this,"Write-off Update Progress"),progressBarEventType:ProgBarEventType.Header),
				cancelButtonText:Lan.g(this,"Close"));
			progress.Fire(ODEventType.FeeSched,new ProgressBarHelper("","0%",0,100,ProgBarStyle.Blocks,"WriteoffProgress"));
			Cursor=Cursors.WaitCursor;
			try {
				FeeScheds.GlobalUpdateWriteoffs(listWriteoffClinics,progress,doUpdatePrevClinicPref);
			}
			catch(ApplicationException ex) {
				Cursor=Cursors.Default;
				progress.Close();
				MessageBox.Show(ex.Message);
			}
			finally {
				if(progress.IsCanceled) {
					progress.Close();
				}
				Cursor=Cursors.Default;
			}
		}

		private void butPickFeeSched_Click(object sender,EventArgs e) {
			//int selectedIndex=GetFeeSchedIndexFromPicker();
			//No need to check security because we are launching the form in selection mode.
			FormFeeScheds FormFS=new FormFeeScheds(true);
			FormFS.ShowDialog();
			int selectedIndex= _listFeeScheds.FindIndex(x => x.FeeSchedNum==FormFS.SelectedFeeSchedNum);//Returns index of the found element or -1.
			//If the selectedIndex is -1, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==-1) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickSched) { //First FeeSched combobox doesn't have "None" option.
				comboFeeSched.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickSchedTo) {
				comboFeeSchedTo.SelectedIndex=selectedIndex;
			}
			FillComboBoxes();
		}

		private void butPickClinic_Click(object sender,EventArgs e){
			if(checkShowGroups.Checked) {
				List<FeeSchedGroup> listGroupsToShow=(comboGroup.Items.AsEnumerable<ODBoxItem<FeeSchedGroup>>()).Select(x => x.Tag).ToList();
				List<GridColumn> listColumnHeaders=new List<GridColumn>() {
					new GridColumn(Lan.g(this,"Description"),-1)
				};
				List<GridRow> listRowValues=new List<GridRow>();
				listGroupsToShow.ForEach(x => {
					GridRow row=new GridRow(x.Description);
					row.Tag=x;
					listRowValues.Add(row);
				});
				string formTitle=Lan.g(this,"Fee Schedule Group Picker");
				string gridTitle=Lan.g(this,"Fee Schedule Groups");
				FormGridSelection form=new FormGridSelection(listColumnHeaders,listRowValues,formTitle,gridTitle);
				if(form.ShowDialog()==DialogResult.OK) {
					comboGroup.SelectedIndex=listGroupsToShow.FindIndex((x => x.FeeSchedGroupNum==((FeeSchedGroup)form.ListSelectedTags[0]).FeeSchedGroupNum));
					return;
				}
				//Nothing was selected. This matches what happens with GetClinicIndexFromPicker.
				return;
			}
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.ListClinics=_listClinics;
			FormC.ShowDialog();
			int selectedIndex=_listClinics.FindIndex(x => x.ClinicNum==FormC.SelectedClinicNum)+1;//All clinic combo boxes have a none option, so always add 1.
			//int selectedIndex=GetClinicIndexFromPicker()+1;
			//If the selectedIndex is 0, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==0) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickClinic) {
				comboClinic.SelectedIndex=selectedIndex;
			}
		}

		private void butPickGroupTo_Click(object sender,EventArgs e) {
			if(checkShowGroups.Checked) {
				List<FeeSchedGroup> listGroupsToShow=(comboGroupTo.Items.AsEnumerable<ODBoxItem<FeeSchedGroup>>()).Select(x => x.Tag).ToList();
				List<GridColumn> listColumnHeaders=new List<GridColumn>() {
					new GridColumn(Lan.g(this,"Description"),-1)
				};
				List<GridRow> listRowValues=new List<GridRow>();
				listGroupsToShow.ForEach(x => {
					GridRow row=new GridRow(x.Description);
					row.Tag=x;
					listRowValues.Add(row);
				});
				string formTitle=Lan.g(this,"Fee Schedule Group Picker");
				string gridTitle=Lan.g(this,"Fee Schedule Groups");
				FormGridSelection form=new FormGridSelection(listColumnHeaders,listRowValues,formTitle,gridTitle);
				if(form.ShowDialog()==DialogResult.OK) {
					comboGroupTo.SelectedIndex=listGroupsToShow.FindIndex((x => x.FeeSchedGroupNum==((FeeSchedGroup)form.ListSelectedTags[0]).FeeSchedGroupNum));
					return;
				}
				//Nothing was selected. This matches what happens with GetClinicIndexFromPicker.
				return;
			}
		}

		private void butPickProvider_Click(object sender,EventArgs e){
			//int selectedIndex=-1;//GetProviderIndexFromPicker()+1;//All provider combo boxes have a none option, so always add 1.
			FormProviderPick FormP=new FormProviderPick();
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;// -1;
			}
			int selectedIndex= Providers.GetIndex(FormP.SelectedProvNum)+1;//All provider combo boxes have a none option, so always add 1.
			//If the selectedIndex is 0, simply return and do not do anything.  There is no such thing as picking 'None' from the picker window.
			if(selectedIndex==0) {
				return;
			}
			UI.Button pickerButton=(UI.Button)sender;
			if(pickerButton==butPickProv) {
				comboProvider.SelectedIndex=selectedIndex;
			}
			else if(pickerButton==butPickProvTo) {
				comboProviderTo.SelectedIndex=selectedIndex;
			}
		}

		private void checkShowGroups_CheckedChanged(object sender,EventArgs e) {
			if(checkShowGroups.Checked) {
				label10.Text="Group";
				label6.Text="Group";
				comboGroup.Visible=true;
				comboGroupTo.Visible=true;
				comboClinic.Visible=false;
				comboClinicTo.Visible=false;
				butPickGroupTo.Visible=true;
			}
			else {
				label10.Text="Clinic";
				label6.Text="Clinic";
				comboGroup.Visible=false;
				comboGroupTo.Visible=false;
				comboClinic.Visible=true;
				comboClinicTo.Visible=true;
				butPickGroupTo.Visible=false;
			}
		}

		///<summary>If either of the FeeSched combos change, we fill the combos.</summary>
//todo:  js Why?
		private void comboFeeCombos_SelectionChangeCommitted(object sender,EventArgs e) {
			FillComboBoxes();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		/*private void FormFeeSchedTools_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.OK && _changed) {
				Cursor=Cursors.WaitCursor;
				_feeCache.SaveToDb();
				SecurityLogs.MakeLogEntries(Permissions.FeeSchedEdit,0,_listSecurityLogEntries);
				Cursor=Cursors.Default;
			}
		}*/
	}
}