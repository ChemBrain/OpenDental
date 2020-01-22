using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using DataConnectionBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	///<summary>Summary description for FormDatabaseMaintenance.</summary>
	public class FormDatabaseMaintenance:ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.TextBox textBox1;
		private OpenDental.UI.Button butCheck;
		private System.ComponentModel.IContainer components;
		private CheckBox checkShow;
		private UI.Button butFix;
		private Label label6;
		private UI.Button butInnoDB;
		private Label label5;
		private Label labelApptProcs;
		private Label label3;
		private Label label2;
		private UI.Button butSpecChar;
		private UI.Button butApptProcs;
		private UI.Button butOptimize;
		private UI.Button butInsPayFix;
		private Label label7;
		private UI.Button butTokens;
		private OpenDental.UI.Button butPrint;
		private Label label8;
		private UI.Button butRemoveNulls;
		private ODGrid gridMain;
		private UI.Button butNone;
		///<summary>Holds any text from the log that still needs to be printed when the log spans multiple pages.</summary>
		private string LogTextPrint;
		private TextBox textBox2;
		///<summary>A list of every single DBM method in the database.  Filled on load right after "syncing" the DBM methods to the db.</summary>
		private List<DatabaseMaintenance> _listDatabaseMaintenances;
		///<summary>This is a filtered list of all methods from DatabaseMaintenances.cs that have the DbmMethod attribute.</summary>
		private List<MethodInfo> _listDbmMethods;
		///<summary>This is a filtered list of methods from DatabaseMaintenances.cs that have the DbmMethod attribute and are not hidden or old.  
		///This is used to populate gridMain.</summary>
		private List<MethodInfo> _listDbmMethodsGrid;
		///<summary>This is a filtered list of methods from DatabaseMaintenances.cs that have the DbmMethod attribute and are hidden.  
		///This is used to populate gridHidden.</summary>
		private List<MethodInfo> _listDbmMethodsGridHidden;
		///<summary>This is a filtered list of methods from DatabaseMaintenances.cs that have the DbmMethod attribute and are marked as old.  
		///This is used to populate gridOld.</summary>
		private List<MethodInfo> _listDbmMethodsGridOld;
		private Label label1;
		private UI.Button butEtrans;
		///<summary>Holds the date and time of the last time a Check or Fix was run.  Only used for printing.</summary>
		private DateTime _dateTimeLastRun;
		private Label label4;
		private UI.Button butActiveTPs;
		private TabControl tabControlDBM;
		private TabPage tabChecks;
		private TabPage tabTools;
		private Label label9;
		private UI.Button butRawEmails;
		private TextBox textBox3;
		private Label labelSkipCheckTable;
		private Label label10;
		private UI.Button butRecalcEst;
		private GroupBox groupBoxUpdateInProg;
		private Label labelUpdateInProgress;
		private TextBox textBoxUpdateInProg;
		private UI.Button butClearUpdateInProgress;
		private TabPage tabHidden;
		private TextBox textBox4;
		private ODGrid gridHidden;
		private TabPage tabOld;
		private TextBox textBox5;
		private ODGrid gridOld;
		private ContextMenuStrip contextMenuStrip1;
		private ToolStripMenuItem hideToolStripMenuItem;
		private ToolStripMenuItem unhideToolStripMenuItem;
		private UI.Button butFixOld;
		private UI.Button butCheckOld;
		private TextBox textNoneOld;
		private UI.Button butNoneOld;
		private UI.Button butSelectAll;
		private Label label11;
		private UI.Button butPayPlanPayments;
		private UI.Button butStopDBM;
		private UI.Button butStopDBMOld;
		///<summary>This bool keeps track of whether we need to invalidate cache for all users.</summary>
		private bool _isCacheInvalid; 
		///<summary>Thread to manage running DBMs, allows us to cancel mid-run.</summary>
		ODThread _threadRunDBM=null;
		private CheckBox checkShowHidden;
		private Label label12;
		private UI.Button butFamilyBalance;
		private Label label13;
		private UI.Button butEmailAttaches;

		/// <summary>Flag to have the RunDBM thread exit early. This should ONLY be set within the main thread and read by the worker thread.</summary>
		private volatile bool _isCancelled;

		///<summary></summary>
		public FormDatabaseMaintenance() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.C(this,new System.Windows.Forms.Control[]{
				this.textBox1,
				//this.textBox2
			});
			Lan.F(this);
		}

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseMaintenance));
			this.butClose = new OpenDental.UI.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.butCheck = new OpenDental.UI.Button();
			this.checkShow = new System.Windows.Forms.CheckBox();
			this.butPrint = new OpenDental.UI.Button();
			this.butFix = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.butActiveTPs = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butEtrans = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.butRemoveNulls = new OpenDental.UI.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.butTokens = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.butInnoDB = new OpenDental.UI.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.labelApptProcs = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butSpecChar = new OpenDental.UI.Button();
			this.butApptProcs = new OpenDental.UI.Button();
			this.butOptimize = new OpenDental.UI.Button();
			this.butInsPayFix = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.butNone = new OpenDental.UI.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.tabControlDBM = new System.Windows.Forms.TabControl();
			this.tabChecks = new System.Windows.Forms.TabPage();
			this.butStopDBM = new OpenDental.UI.Button();
			this.tabHidden = new System.Windows.Forms.TabPage();
			this.butSelectAll = new OpenDental.UI.Button();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.gridHidden = new OpenDental.UI.ODGrid();
			this.tabOld = new System.Windows.Forms.TabPage();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.butStopDBMOld = new OpenDental.UI.Button();
			this.textNoneOld = new System.Windows.Forms.TextBox();
			this.butNoneOld = new OpenDental.UI.Button();
			this.butFixOld = new OpenDental.UI.Button();
			this.butCheckOld = new OpenDental.UI.Button();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.gridOld = new OpenDental.UI.ODGrid();
			this.tabTools = new System.Windows.Forms.TabPage();
			this.label12 = new System.Windows.Forms.Label();
			this.butFamilyBalance = new OpenDental.UI.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.butPayPlanPayments = new OpenDental.UI.Button();
			this.groupBoxUpdateInProg = new System.Windows.Forms.GroupBox();
			this.labelUpdateInProgress = new System.Windows.Forms.Label();
			this.textBoxUpdateInProg = new System.Windows.Forms.TextBox();
			this.butClearUpdateInProgress = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.butRecalcEst = new OpenDental.UI.Button();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.butRawEmails = new OpenDental.UI.Button();
			this.labelSkipCheckTable = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.butEmailAttaches = new OpenDental.UI.Button();
			this.contextMenuStrip1.SuspendLayout();
			this.tabControlDBM.SuspendLayout();
			this.tabChecks.SuspendLayout();
			this.tabHidden.SuspendLayout();
			this.tabOld.SuspendLayout();
			this.tabTools.SuspendLayout();
			this.groupBoxUpdateInProg.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(737, 654);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(6, 6);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(779, 40);
			this.textBox1.TabIndex = 1;
			this.textBox1.TabStop = false;
			this.textBox1.Text = "This tool will check the entire database for any improper settings, inconsistenci" +
    "es, or corruption.\r\nA log is automatically saved in RepairLog.txt if user has pe" +
    "rmission.";
			// 
			// butCheck
			// 
			this.butCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butCheck.Location = new System.Drawing.Point(301, 534);
			this.butCheck.Name = "butCheck";
			this.butCheck.Size = new System.Drawing.Size(75, 26);
			this.butCheck.TabIndex = 4;
			this.butCheck.Text = "C&heck";
			this.butCheck.Click += new System.EventHandler(this.butCheck_Click);
			// 
			// checkShow
			// 
			this.checkShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShow.Location = new System.Drawing.Point(6, 479);
			this.checkShow.Name = "checkShow";
			this.checkShow.Size = new System.Drawing.Size(447, 20);
			this.checkShow.TabIndex = 1;
			this.checkShow.Text = "Show me everything in the log  (only for advanced users)";
			// 
			// butPrint
			// 
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(6, 534);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(87, 26);
			this.butPrint.TabIndex = 3;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butFix
			// 
			this.butFix.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butFix.Location = new System.Drawing.Point(426, 534);
			this.butFix.Name = "butFix";
			this.butFix.Size = new System.Drawing.Size(75, 26);
			this.butFix.TabIndex = 5;
			this.butFix.Text = "&Fix";
			this.butFix.Click += new System.EventHandler(this.butFix_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(150, 409);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(631, 20);
			this.label4.TabIndex = 48;
			this.label4.Text = "Creates an active treatment plan for all pats with treatment planned procs.";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butActiveTPs
			// 
			this.butActiveTPs.Location = new System.Drawing.Point(30, 405);
			this.butActiveTPs.Name = "butActiveTPs";
			this.butActiveTPs.Size = new System.Drawing.Size(114, 26);
			this.butActiveTPs.TabIndex = 8;
			this.butActiveTPs.Text = "Active TPs";
			this.butActiveTPs.Click += new System.EventHandler(this.butActiveTPs_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(150, 377);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(631, 20);
			this.label1.TabIndex = 46;
			this.label1.Text = "Clear out etrans entries older than a year old.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butEtrans
			// 
			this.butEtrans.Enabled = false;
			this.butEtrans.Location = new System.Drawing.Point(30, 373);
			this.butEtrans.Name = "butEtrans";
			this.butEtrans.Size = new System.Drawing.Size(114, 26);
			this.butEtrans.TabIndex = 7;
			this.butEtrans.Text = "Etrans";
			this.butEtrans.Click += new System.EventHandler(this.butEtrans_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(150, 345);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(631, 20);
			this.label8.TabIndex = 44;
			this.label8.Text = "Replace all null strings with empty strings.";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butRemoveNulls
			// 
			this.butRemoveNulls.Location = new System.Drawing.Point(30, 341);
			this.butRemoveNulls.Name = "butRemoveNulls";
			this.butRemoveNulls.Size = new System.Drawing.Size(114, 26);
			this.butRemoveNulls.TabIndex = 6;
			this.butRemoveNulls.Text = "Remove Nulls";
			this.butRemoveNulls.Click += new System.EventHandler(this.butRemoveNulls_Click);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(150, 313);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(631, 20);
			this.label7.TabIndex = 42;
			this.label7.Text = "Validates tokens on file with the X-Charge server.";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butTokens
			// 
			this.butTokens.Location = new System.Drawing.Point(30, 309);
			this.butTokens.Name = "butTokens";
			this.butTokens.Size = new System.Drawing.Size(114, 26);
			this.butTokens.TabIndex = 5;
			this.butTokens.Text = "Tokens";
			this.butTokens.Click += new System.EventHandler(this.butTokens_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(150, 280);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(631, 20);
			this.label6.TabIndex = 40;
			this.label6.Text = "Converts database storage engine to/from InnoDb.";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butInnoDB
			// 
			this.butInnoDB.Location = new System.Drawing.Point(30, 277);
			this.butInnoDB.Name = "butInnoDB";
			this.butInnoDB.Size = new System.Drawing.Size(114, 26);
			this.butInnoDB.TabIndex = 4;
			this.butInnoDB.Text = "InnoDb";
			this.butInnoDB.Click += new System.EventHandler(this.butInnoDB_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(150, 248);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(631, 20);
			this.label5.TabIndex = 38;
			this.label5.Text = "Removes special characters from appt notes and appt proc descriptions.";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelApptProcs
			// 
			this.labelApptProcs.Location = new System.Drawing.Point(150, 216);
			this.labelApptProcs.Name = "labelApptProcs";
			this.labelApptProcs.Size = new System.Drawing.Size(631, 20);
			this.labelApptProcs.TabIndex = 37;
			this.labelApptProcs.Text = "Fixes procs in the Appt module that aren\'t correctly showing tooth nums.";
			this.labelApptProcs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(150, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(631, 20);
			this.label3.TabIndex = 36;
			this.label3.Text = "Back up, optimize, and repair tables.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(150, 152);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(631, 20);
			this.label2.TabIndex = 35;
			this.label2.Text = "Creates checks for insurance payments that are not attached to a check.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butSpecChar
			// 
			this.butSpecChar.Location = new System.Drawing.Point(30, 245);
			this.butSpecChar.Name = "butSpecChar";
			this.butSpecChar.Size = new System.Drawing.Size(114, 26);
			this.butSpecChar.TabIndex = 3;
			this.butSpecChar.Text = "Spec Char";
			this.butSpecChar.Click += new System.EventHandler(this.butSpecChar_Click);
			// 
			// butApptProcs
			// 
			this.butApptProcs.Location = new System.Drawing.Point(30, 213);
			this.butApptProcs.Name = "butApptProcs";
			this.butApptProcs.Size = new System.Drawing.Size(114, 26);
			this.butApptProcs.TabIndex = 2;
			this.butApptProcs.Text = "Appt Procs";
			this.butApptProcs.Click += new System.EventHandler(this.butApptProcs_Click);
			// 
			// butOptimize
			// 
			this.butOptimize.Location = new System.Drawing.Point(30, 181);
			this.butOptimize.Name = "butOptimize";
			this.butOptimize.Size = new System.Drawing.Size(114, 26);
			this.butOptimize.TabIndex = 1;
			this.butOptimize.Text = "Optimize";
			this.butOptimize.Click += new System.EventHandler(this.butOptimize_Click);
			// 
			// butInsPayFix
			// 
			this.butInsPayFix.Location = new System.Drawing.Point(30, 149);
			this.butInsPayFix.Name = "butInsPayFix";
			this.butInsPayFix.Size = new System.Drawing.Size(114, 26);
			this.butInsPayFix.TabIndex = 0;
			this.butInsPayFix.Text = "Ins Pay Fix";
			this.butInsPayFix.Click += new System.EventHandler(this.butInsPayFix_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.ContextMenuStrip = this.contextMenuStrip1;
			this.gridMain.HasMultilineHeaders = true;
			this.gridMain.Location = new System.Drawing.Point(6, 52);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(790, 421);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Database Methods";
			this.gridMain.TranslationName = "TableClaimPaySplits";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridMain_MouseUp);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideToolStripMenuItem,
            this.unhideToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(113, 48);
			// 
			// hideToolStripMenuItem
			// 
			this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
			this.hideToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.hideToolStripMenuItem.Text = "Hide";
			this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
			// 
			// unhideToolStripMenuItem
			// 
			this.unhideToolStripMenuItem.Name = "unhideToolStripMenuItem";
			this.unhideToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
			this.unhideToolStripMenuItem.Text = "Unhide";
			this.unhideToolStripMenuItem.Click += new System.EventHandler(this.unhideToolStripMenuItem_Click);
			// 
			// butNone
			// 
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNone.Location = new System.Drawing.Point(721, 479);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(75, 26);
			this.butNone.TabIndex = 2;
			this.butNone.Text = "None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox2.BackColor = System.Drawing.SystemColors.Control;
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox2.Location = new System.Drawing.Point(350, 479);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(365, 26);
			this.textBox2.TabIndex = 99;
			this.textBox2.TabStop = false;
			this.textBox2.Text = "No selections will cause all database methods to run.\r\nOtherwise only selected me" +
    "thods will run.\r\n";
			this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// tabControlDBM
			// 
			this.tabControlDBM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlDBM.Controls.Add(this.tabChecks);
			this.tabControlDBM.Controls.Add(this.tabHidden);
			this.tabControlDBM.Controls.Add(this.tabOld);
			this.tabControlDBM.Controls.Add(this.tabTools);
			this.tabControlDBM.Location = new System.Drawing.Point(12, 12);
			this.tabControlDBM.Name = "tabControlDBM";
			this.tabControlDBM.SelectedIndex = 0;
			this.tabControlDBM.Size = new System.Drawing.Size(810, 627);
			this.tabControlDBM.TabIndex = 0;
			// 
			// tabChecks
			// 
			this.tabChecks.BackColor = System.Drawing.SystemColors.Control;
			this.tabChecks.Controls.Add(this.butStopDBM);
			this.tabChecks.Controls.Add(this.textBox1);
			this.tabChecks.Controls.Add(this.butFix);
			this.tabChecks.Controls.Add(this.butPrint);
			this.tabChecks.Controls.Add(this.textBox2);
			this.tabChecks.Controls.Add(this.butCheck);
			this.tabChecks.Controls.Add(this.checkShow);
			this.tabChecks.Controls.Add(this.butNone);
			this.tabChecks.Controls.Add(this.gridMain);
			this.tabChecks.Location = new System.Drawing.Point(4, 22);
			this.tabChecks.Name = "tabChecks";
			this.tabChecks.Padding = new System.Windows.Forms.Padding(3);
			this.tabChecks.Size = new System.Drawing.Size(802, 566);
			this.tabChecks.TabIndex = 0;
			this.tabChecks.Text = "Checks";
			// 
			// butStopDBM
			// 
			this.butStopDBM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butStopDBM.Enabled = false;
			this.butStopDBM.Location = new System.Drawing.Point(545, 534);
			this.butStopDBM.Name = "butStopDBM";
			this.butStopDBM.Size = new System.Drawing.Size(75, 26);
			this.butStopDBM.TabIndex = 6;
			this.butStopDBM.Text = "&Stop DBM";
			this.butStopDBM.Click += new System.EventHandler(this.butStopDBM_Click);
			// 
			// tabHidden
			// 
			this.tabHidden.BackColor = System.Drawing.Color.Transparent;
			this.tabHidden.Controls.Add(this.butSelectAll);
			this.tabHidden.Controls.Add(this.textBox4);
			this.tabHidden.Controls.Add(this.gridHidden);
			this.tabHidden.Location = new System.Drawing.Point(4, 22);
			this.tabHidden.Name = "tabHidden";
			this.tabHidden.Padding = new System.Windows.Forms.Padding(3);
			this.tabHidden.Size = new System.Drawing.Size(802, 566);
			this.tabHidden.TabIndex = 2;
			this.tabHidden.Text = "Hidden";
			// 
			// butSelectAll
			// 
			this.butSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSelectAll.Location = new System.Drawing.Point(721, 470);
			this.butSelectAll.Name = "butSelectAll";
			this.butSelectAll.Size = new System.Drawing.Size(75, 26);
			this.butSelectAll.TabIndex = 101;
			this.butSelectAll.Text = "Select All";
			this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
			// 
			// textBox4
			// 
			this.textBox4.BackColor = System.Drawing.SystemColors.Control;
			this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox4.Location = new System.Drawing.Point(6, 3);
			this.textBox4.Multiline = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.ReadOnly = true;
			this.textBox4.Size = new System.Drawing.Size(779, 40);
			this.textBox4.TabIndex = 3;
			this.textBox4.TabStop = false;
			this.textBox4.Text = "This table shows all of the hidden database maintenance methods. You can unhide a" +
    " method by selecting a method, right clicking, and select Unhide.\r\n\r\n";
			// 
			// gridHidden
			// 
			this.gridHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridHidden.ContextMenuStrip = this.contextMenuStrip1;
			this.gridHidden.HasMultilineHeaders = true;
			this.gridHidden.Location = new System.Drawing.Point(6, 52);
			this.gridHidden.Name = "gridHidden";
			this.gridHidden.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridHidden.Size = new System.Drawing.Size(790, 412);
			this.gridHidden.TabIndex = 2;
			this.gridHidden.Title = "Hidden Methods";
			this.gridHidden.TranslationName = "TableHiddenDbmMethods";
			this.gridHidden.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridHidden_MouseUp);
			// 
			// tabOld
			// 
			this.tabOld.BackColor = System.Drawing.Color.Transparent;
			this.tabOld.Controls.Add(this.checkShowHidden);
			this.tabOld.Controls.Add(this.butStopDBMOld);
			this.tabOld.Controls.Add(this.textNoneOld);
			this.tabOld.Controls.Add(this.butNoneOld);
			this.tabOld.Controls.Add(this.butFixOld);
			this.tabOld.Controls.Add(this.butCheckOld);
			this.tabOld.Controls.Add(this.textBox5);
			this.tabOld.Controls.Add(this.gridOld);
			this.tabOld.Location = new System.Drawing.Point(4, 22);
			this.tabOld.Name = "tabOld";
			this.tabOld.Padding = new System.Windows.Forms.Padding(3);
			this.tabOld.Size = new System.Drawing.Size(802, 566);
			this.tabOld.TabIndex = 3;
			this.tabOld.Text = "Old";
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShowHidden.Location = new System.Drawing.Point(6, 537);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(134, 17);
			this.checkShowHidden.TabIndex = 103;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.UseVisualStyleBackColor = true;
			this.checkShowHidden.CheckedChanged += new System.EventHandler(this.checkShowHidden_CheckedChanged);
			// 
			// butStopDBMOld
			// 
			this.butStopDBMOld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butStopDBMOld.Enabled = false;
			this.butStopDBMOld.Location = new System.Drawing.Point(545, 525);
			this.butStopDBMOld.Name = "butStopDBMOld";
			this.butStopDBMOld.Size = new System.Drawing.Size(75, 26);
			this.butStopDBMOld.TabIndex = 102;
			this.butStopDBMOld.Text = "&Stop DBM";
			this.butStopDBMOld.Click += new System.EventHandler(this.butStopDBMOld_Click);
			// 
			// textNoneOld
			// 
			this.textNoneOld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textNoneOld.BackColor = System.Drawing.SystemColors.Control;
			this.textNoneOld.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textNoneOld.Location = new System.Drawing.Point(350, 470);
			this.textNoneOld.Multiline = true;
			this.textNoneOld.Name = "textNoneOld";
			this.textNoneOld.ReadOnly = true;
			this.textNoneOld.Size = new System.Drawing.Size(365, 26);
			this.textNoneOld.TabIndex = 101;
			this.textNoneOld.TabStop = false;
			this.textNoneOld.Text = "No selections will cause all database checks to run.\r\nOtherwise only selected che" +
    "cks will run.\r\n";
			this.textNoneOld.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butNoneOld
			// 
			this.butNoneOld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNoneOld.Location = new System.Drawing.Point(721, 470);
			this.butNoneOld.Name = "butNoneOld";
			this.butNoneOld.Size = new System.Drawing.Size(75, 26);
			this.butNoneOld.TabIndex = 100;
			this.butNoneOld.Text = "None";
			this.butNoneOld.Click += new System.EventHandler(this.butNoneOld_Click);
			// 
			// butFixOld
			// 
			this.butFixOld.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butFixOld.Location = new System.Drawing.Point(423, 525);
			this.butFixOld.Name = "butFixOld";
			this.butFixOld.Size = new System.Drawing.Size(75, 26);
			this.butFixOld.TabIndex = 7;
			this.butFixOld.Text = "&Fix";
			this.butFixOld.Click += new System.EventHandler(this.butFixOld_Click);
			// 
			// butCheckOld
			// 
			this.butCheckOld.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butCheckOld.Location = new System.Drawing.Point(298, 525);
			this.butCheckOld.Name = "butCheckOld";
			this.butCheckOld.Size = new System.Drawing.Size(75, 26);
			this.butCheckOld.TabIndex = 6;
			this.butCheckOld.Text = "C&heck";
			this.butCheckOld.Click += new System.EventHandler(this.butCheckOld_Click);
			// 
			// textBox5
			// 
			this.textBox5.BackColor = System.Drawing.SystemColors.Control;
			this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox5.Location = new System.Drawing.Point(6, 3);
			this.textBox5.Multiline = true;
			this.textBox5.Name = "textBox5";
			this.textBox5.ReadOnly = true;
			this.textBox5.Size = new System.Drawing.Size(779, 40);
			this.textBox5.TabIndex = 5;
			this.textBox5.TabStop = false;
			this.textBox5.Text = "This table shows database maintenance methods that have been deemed no longer nec" +
    "essary. Should not be ran unless directly told to do so.\r\n\r\n";
			// 
			// gridOld
			// 
			this.gridOld.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridOld.ContextMenuStrip = this.contextMenuStrip1;
			this.gridOld.HasMultilineHeaders = true;
			this.gridOld.Location = new System.Drawing.Point(6, 52);
			this.gridOld.Name = "gridOld";
			this.gridOld.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridOld.Size = new System.Drawing.Size(790, 412);
			this.gridOld.TabIndex = 4;
			this.gridOld.Title = "Old Methods";
			this.gridOld.TranslationName = "TableOldDbmMethods";
			this.gridOld.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridOld_CellDoubleClick);
			this.gridOld.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridOld_MouseUp);
			// 
			// tabTools
			// 
			this.tabTools.BackColor = System.Drawing.SystemColors.Control;
			this.tabTools.Controls.Add(this.label13);
			this.tabTools.Controls.Add(this.butEmailAttaches);
			this.tabTools.Controls.Add(this.label12);
			this.tabTools.Controls.Add(this.butFamilyBalance);
			this.tabTools.Controls.Add(this.label11);
			this.tabTools.Controls.Add(this.butPayPlanPayments);
			this.tabTools.Controls.Add(this.groupBoxUpdateInProg);
			this.tabTools.Controls.Add(this.label10);
			this.tabTools.Controls.Add(this.butRecalcEst);
			this.tabTools.Controls.Add(this.textBox3);
			this.tabTools.Controls.Add(this.label9);
			this.tabTools.Controls.Add(this.butRawEmails);
			this.tabTools.Controls.Add(this.label4);
			this.tabTools.Controls.Add(this.butActiveTPs);
			this.tabTools.Controls.Add(this.butInsPayFix);
			this.tabTools.Controls.Add(this.label1);
			this.tabTools.Controls.Add(this.butOptimize);
			this.tabTools.Controls.Add(this.butEtrans);
			this.tabTools.Controls.Add(this.butApptProcs);
			this.tabTools.Controls.Add(this.label8);
			this.tabTools.Controls.Add(this.butSpecChar);
			this.tabTools.Controls.Add(this.butRemoveNulls);
			this.tabTools.Controls.Add(this.label2);
			this.tabTools.Controls.Add(this.label7);
			this.tabTools.Controls.Add(this.label3);
			this.tabTools.Controls.Add(this.butTokens);
			this.tabTools.Controls.Add(this.labelApptProcs);
			this.tabTools.Controls.Add(this.label6);
			this.tabTools.Controls.Add(this.label5);
			this.tabTools.Controls.Add(this.butInnoDB);
			this.tabTools.Location = new System.Drawing.Point(4, 22);
			this.tabTools.Name = "tabTools";
			this.tabTools.Padding = new System.Windows.Forms.Padding(3);
			this.tabTools.Size = new System.Drawing.Size(802, 601);
			this.tabTools.TabIndex = 1;
			this.tabTools.Text = "Tools";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(150, 568);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(455, 20);
			this.label12.TabIndex = 61;
			this.label12.Text = "Runs income transfer logic for multiple familes at once to zero out family balanc" +
    "es.";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butFamilyBalance
			// 
			this.butFamilyBalance.Location = new System.Drawing.Point(30, 565);
			this.butFamilyBalance.Name = "butFamilyBalance";
			this.butFamilyBalance.Size = new System.Drawing.Size(114, 26);
			this.butFamilyBalance.TabIndex = 60;
			this.butFamilyBalance.Text = "Balance Families";
			this.butFamilyBalance.Click += new System.EventHandler(this.butFamilyBalance_Click);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(150, 536);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(631, 20);
			this.label11.TabIndex = 59;
			this.label11.Text = "Detaches patient payments attached to insurance payment plans and insurance payme" +
    "nts attached to patient payment plans.";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butPayPlanPayments
			// 
			this.butPayPlanPayments.Location = new System.Drawing.Point(30, 533);
			this.butPayPlanPayments.Name = "butPayPlanPayments";
			this.butPayPlanPayments.Size = new System.Drawing.Size(114, 26);
			this.butPayPlanPayments.TabIndex = 58;
			this.butPayPlanPayments.Text = "Pay Plan Payments";
			this.butPayPlanPayments.Click += new System.EventHandler(this.butPayPlanPayments_Click);
			// 
			// groupBoxUpdateInProg
			// 
			this.groupBoxUpdateInProg.Controls.Add(this.labelUpdateInProgress);
			this.groupBoxUpdateInProg.Controls.Add(this.textBoxUpdateInProg);
			this.groupBoxUpdateInProg.Controls.Add(this.butClearUpdateInProgress);
			this.groupBoxUpdateInProg.Location = new System.Drawing.Point(6, 8);
			this.groupBoxUpdateInProg.Name = "groupBoxUpdateInProg";
			this.groupBoxUpdateInProg.Size = new System.Drawing.Size(605, 78);
			this.groupBoxUpdateInProg.TabIndex = 57;
			this.groupBoxUpdateInProg.TabStop = false;
			this.groupBoxUpdateInProg.Text = "Update in progress on computer: ";
			// 
			// labelUpdateInProgress
			// 
			this.labelUpdateInProgress.Location = new System.Drawing.Point(21, 17);
			this.labelUpdateInProgress.Name = "labelUpdateInProgress";
			this.labelUpdateInProgress.Size = new System.Drawing.Size(578, 26);
			this.labelUpdateInProgress.TabIndex = 58;
			this.labelUpdateInProgress.Text = "Clear this value only if you are unable to start the program on other workstation" +
    "s and you are sure an update is not currently in progress.";
			// 
			// textBoxUpdateInProg
			// 
			this.textBoxUpdateInProg.Location = new System.Drawing.Point(24, 47);
			this.textBoxUpdateInProg.Name = "textBoxUpdateInProg";
			this.textBoxUpdateInProg.ReadOnly = true;
			this.textBoxUpdateInProg.Size = new System.Drawing.Size(149, 20);
			this.textBoxUpdateInProg.TabIndex = 55;
			// 
			// butClearUpdateInProgress
			// 
			this.butClearUpdateInProgress.Location = new System.Drawing.Point(179, 45);
			this.butClearUpdateInProgress.Name = "butClearUpdateInProgress";
			this.butClearUpdateInProgress.Size = new System.Drawing.Size(78, 23);
			this.butClearUpdateInProgress.TabIndex = 54;
			this.butClearUpdateInProgress.Text = "Clear";
			this.butClearUpdateInProgress.UseVisualStyleBackColor = true;
			this.butClearUpdateInProgress.Click += new System.EventHandler(this.butClearUpdateInProgress_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(150, 504);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(631, 20);
			this.label10.TabIndex = 53;
			this.label10.Text = "Recalc estimates that are associated to non active coverage for the patient.";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butRecalcEst
			// 
			this.butRecalcEst.Location = new System.Drawing.Point(30, 501);
			this.butRecalcEst.Name = "butRecalcEst";
			this.butRecalcEst.Size = new System.Drawing.Size(114, 26);
			this.butRecalcEst.TabIndex = 52;
			this.butRecalcEst.Text = "Recalc Estimates";
			this.butRecalcEst.Click += new System.EventHandler(this.butRecalcEst_Click);
			// 
			// textBox3
			// 
			this.textBox3.BackColor = System.Drawing.SystemColors.Control;
			this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox3.Location = new System.Drawing.Point(6, 92);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.ReadOnly = true;
			this.textBox3.Size = new System.Drawing.Size(790, 54);
			this.textBox3.TabIndex = 51;
			this.textBox3.TabStop = false;
			this.textBox3.Text = resources.GetString("textBox3.Text");
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(150, 441);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(631, 20);
			this.label9.TabIndex = 50;
			this.label9.Text = "Fixes emails which are encoded in the Chart progress notes.  Also clears unused a" +
    "ttachments.";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butRawEmails
			// 
			this.butRawEmails.Location = new System.Drawing.Point(30, 437);
			this.butRawEmails.Name = "butRawEmails";
			this.butRawEmails.Size = new System.Drawing.Size(114, 26);
			this.butRawEmails.TabIndex = 9;
			this.butRawEmails.Text = "Raw Emails";
			this.butRawEmails.Click += new System.EventHandler(this.butRawEmails_Click);
			// 
			// labelSkipCheckTable
			// 
			this.labelSkipCheckTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSkipCheckTable.ForeColor = System.Drawing.Color.Red;
			this.labelSkipCheckTable.Location = new System.Drawing.Point(587, 661);
			this.labelSkipCheckTable.Name = "labelSkipCheckTable";
			this.labelSkipCheckTable.Size = new System.Drawing.Size(144, 16);
			this.labelSkipCheckTable.TabIndex = 2;
			this.labelSkipCheckTable.Text = "Table check is disabled";
			this.labelSkipCheckTable.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.labelSkipCheckTable.Visible = false;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(150, 473);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(631, 20);
			this.label13.TabIndex = 63;
			this.label13.Text = "Moves email attachment files into the correct In or Out folders.";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butEmailAttaches
			// 
			this.butEmailAttaches.Location = new System.Drawing.Point(30, 469);
			this.butEmailAttaches.Name = "butEmailAttaches";
			this.butEmailAttaches.Size = new System.Drawing.Size(114, 26);
			this.butEmailAttaches.TabIndex = 62;
			this.butEmailAttaches.Text = "Email Attaches";
			this.butEmailAttaches.Click += new System.EventHandler(this.butEmailAttaches_Click);
			// 
			// FormDatabaseMaintenance
			// 
			this.AcceptButton = this.butCheck;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(834, 692);
			this.Controls.Add(this.labelSkipCheckTable);
			this.Controls.Add(this.tabControlDBM);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(850, 731);
			this.Name = "FormDatabaseMaintenance";
			this.ShowInTaskbar = false;
			this.Text = "Database Maintenance";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDatabaseMaintenance_FormClosing);
			this.Load += new System.EventHandler(this.FormDatabaseMaintenance_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.tabControlDBM.ResumeLayout(false);
			this.tabChecks.ResumeLayout(false);
			this.tabChecks.PerformLayout();
			this.tabHidden.ResumeLayout(false);
			this.tabHidden.PerformLayout();
			this.tabOld.ResumeLayout(false);
			this.tabOld.PerformLayout();
			this.tabTools.ResumeLayout(false);
			this.tabTools.PerformLayout();
			this.groupBoxUpdateInProg.ResumeLayout(false);
			this.groupBoxUpdateInProg.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormDatabaseMaintenance_Load(object sender,System.EventArgs e) {
			_listDbmMethods=DatabaseMaintenances.GetMethodsForDisplay(Clinics.ClinicNum);
			DatabaseMaintenances.InsertMissingDBMs(_listDbmMethods.Select(x => x.Name).ToList());
			_listDatabaseMaintenances=DatabaseMaintenances.GetAll();
			//Users get stopped from launching FormDatabaseMaintenance when they do not have the Setup permission.
			//Jordan wants some tools to only be accessible to users with the SecurityAdmin permission.
			if(Security.IsAuthorized(Permissions.SecurityAdmin,true)){
				butEtrans.Enabled=true;
			}
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				butApptProcs.Visible=false;
				labelApptProcs.Visible=false;
			}
			if(PrefC.GetBool(PrefName.DatabaseMaintenanceDisableOptimize)) {
				butOptimize.Enabled=false;
			}
			if(PrefC.GetBool(PrefName.DatabaseMaintenanceSkipCheckTable)) {
				labelSkipCheckTable.Visible=true;
			}
			FillGrid();
			FillGridHidden();
			FillGridOld();
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				butRemoveNulls.Visible=false;
			}
			textBoxUpdateInProg.Text=PrefC.GetString(PrefName.UpdateInProgressOnComputerName);
			if(string.IsNullOrWhiteSpace(textBoxUpdateInProg.Text)) {
				butClearUpdateInProgress.Enabled=false;
			}
		}

		private void FillGrid() {
			_listDbmMethodsGrid=GetDbmMethodsForGrid();
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g(this,"Name"),300));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g(this,"Break\r\nDown"),40,HorizontalAlignment.Center));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g(this,"Results"),0));
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<_listDbmMethodsGrid.Count;i++) {
				row=new GridRow();
				row.Cells.Add(_listDbmMethodsGrid[i].Name);
				row.Cells.Add(DatabaseMaintenances.MethodHasBreakDown(_listDbmMethodsGrid[i]) ? "X" : "");
				row.Cells.Add("");
				row.Tag=_listDbmMethodsGrid[i];
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillGridHidden() {
			_listDbmMethodsGridHidden=GetDbmMethodsForGrid(isHidden: true,isOld: false);
			gridHidden.BeginUpdate();
			gridHidden.ListGridColumns.Clear();
			gridHidden.ListGridColumns.Add(new GridColumn(Lan.g(this,"Name"),340));
			gridHidden.ListGridRows.Clear();
			GridRow row;
			for(int i = 0;i<_listDbmMethodsGridHidden.Count;i++) {
				row=new GridRow();
				row.Cells.Add(_listDbmMethodsGridHidden[i].Name);
				row.Tag=_listDbmMethodsGridHidden[i];
				gridHidden.ListGridRows.Add(row);
			}
			gridHidden.EndUpdate();
		}

		private void FillGridOld() {
			_listDbmMethodsGridOld=GetDbmMethodsForGrid(isHidden: false,isOld: true);
			_listDbmMethodsGridOld.AddRange(GetDbmMethodsForGrid(isHidden: true,isOld: true));
			_listDbmMethodsGridOld.Sort(new MethodInfoComparer());
			gridOld.BeginUpdate();
			gridOld.ListGridColumns.Clear();
			gridOld.ListGridColumns.Add(new GridColumn(Lan.g(this,"Name"),300));
			if(checkShowHidden.Checked) {
				gridOld.ListGridColumns.Add(new GridColumn(Lan.g(this,"Hidden"),45,HorizontalAlignment.Center));
			}
			gridOld.ListGridColumns.Add(new GridColumn(Lan.g(this,"Break\r\nDown"),40,HorizontalAlignment.Center));
			gridOld.ListGridColumns.Add(new GridColumn(Lan.g(this,"Results"),0));
			gridOld.ListGridRows.Clear();
			GridRow row;
			for(int i = 0;i<_listDbmMethodsGridOld.Count;i++) {
				bool isMethodHidden=_listDatabaseMaintenances.Any(x => x.MethodName==_listDbmMethodsGridOld[i].Name && x.IsHidden);
				if(!checkShowHidden.Checked && isMethodHidden) {
					continue;
				}
				row=new GridRow();
				row.Cells.Add(_listDbmMethodsGridOld[i].Name);
				if(checkShowHidden.Checked) {
					row.Cells.Add(isMethodHidden ? "X" : "");
				}
				row.Cells.Add(DatabaseMaintenances.MethodHasBreakDown(_listDbmMethodsGridOld[i]) ? "X" : "");
				row.Cells.Add("");
				row.Tag=_listDbmMethodsGridOld[i];
				gridOld.ListGridRows.Add(row);
			}
			gridOld.EndUpdate();
		}

		private List<MethodInfo> GetDbmMethodsForGrid(bool isHidden=false,bool isOld=false) {
			List<string> listMethods=_listDatabaseMaintenances.FindAll(x => x.IsHidden==isHidden && x.IsOld==isOld)
				.Select(y => y.MethodName).ToList();
			return _listDbmMethods.FindAll(x => x.Name.In(listMethods));
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			RunMethodBreakDown(_listDbmMethodsGrid[e.Row]);
		}

		private void gridOld_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			RunMethodBreakDown(_listDbmMethodsGridOld[e.Row]);
		}

		private void gridMain_MouseUp(object sender,MouseEventArgs e) {
			OnMouseUp(e,gridMain);
		}

		private void gridHidden_MouseUp(object sender,MouseEventArgs e) {
			OnMouseUp(e,gridHidden);
		}

		private void gridOld_MouseUp(object sender,MouseEventArgs e) {
			OnMouseUp(e,gridOld);
		}

		private void OnMouseUp(MouseEventArgs e,ODGrid grid) {
			if(grid.SelectedIndices.Length==0 || e.Button!=MouseButtons.Right) {
				contextMenuStrip1.Hide();
				return;
			}
			MethodInfo method=(MethodInfo)grid.ListGridRows[grid.SelectedIndices[0]].Tag;
			if(method!=null) {
				bool isMethodHidden=_listDatabaseMaintenances.Any(x => x.MethodName==method.Name && x.IsHidden);
				hideToolStripMenuItem.Visible=!isMethodHidden;
				unhideToolStripMenuItem.Visible=isMethodHidden;
			}
		}

		private void hideToolStripMenuItem_Click(object sender,EventArgs e) {
			//Users can only hide DBM methods from gridMain or gridOld.
			switch(tabControlDBM.SelectedIndex) {
				case 0://tabChecks
					UpdateDbmIsHiddenForGrid(gridMain,true);
					break;
				case 2://tabOld
					UpdateDbmIsHiddenForGrid(gridOld,true);
					break;
				case 1://tabHidden
				case 3://tabTools
				default:
					return;
			}
		}

		private void unhideToolStripMenuItem_Click(object sender,EventArgs e) {
			//Users can only unhide DBM methods from gridHidden or gridOld.
			switch(tabControlDBM.SelectedIndex) {
				case 1://tabHidden
					UpdateDbmIsHiddenForGrid(gridHidden,false);
					break;
				case 2://tabOld
					UpdateDbmIsHiddenForGrid(gridOld,false);
					break;
				case 0://tabChecks
				case 3://tabTools
				default:
					return;
			}
		}

		private void UpdateDbmIsHiddenForGrid(ODGrid grid,bool isHidden) {
			for(int i=0;i<grid.SelectedIndices.Length;i++) {
				MethodInfo method=(MethodInfo)grid.ListGridRows[grid.SelectedIndices[i]].Tag;
				DatabaseMaintenance dbm=_listDatabaseMaintenances.FirstOrDefault(x=>x.MethodName==method.Name);
				if(dbm==null) {
					continue;
				}
				dbm.IsHidden=isHidden;
				DatabaseMaintenances.Update(dbm);
			}
			_listDatabaseMaintenances=DatabaseMaintenances.GetAll();
			FillGrid();
			FillGridHidden();
			FillGridOld();
		}

		private void butNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
		}

		private void butNoneOld_Click(object sender,EventArgs e) {
			gridOld.SetSelected(false);
		}

		private void butSelectAll_Click(object sender,EventArgs e) {
			gridHidden.SetSelected(true);
		}

		#region Database Tools

		private void butClearUpdateInProgress_Click(object sender,EventArgs e) {
			Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,"");
			DataValid.SetInvalid(InvalidType.Prefs);
			textBoxUpdateInProg.Text="";
		}

		private void butInsPayFix_Click(object sender,EventArgs e) {
			FormInsPayFix formIns=new FormInsPayFix();
			formIns.ShowDialog();
		}

		private void butOptimize_Click(object sender,EventArgs e) {
			if(MessageBox.Show(Lan.g("FormDatabaseMaintenance","This tool will backup, optimize, and repair all tables.")+"\r\n"+Lan.g("FormDatabaseMaintenance","Continue?")
				,Lan.g("FormDatabaseMaintenance","Backup Optimize Repair")
				,MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			string result="";
			if(Shared.BackupRepairAndOptimize(true,BackupLocation.OptimizeTool)) {
				result=DateTime.Now.ToString()+"\r\n"+Lan.g("FormDatabaseMaintenance","Repair and Optimization Complete");
			}
			else {
				result=DateTime.Now.ToString()+"\r\n";
				result+=Lan.g("FormDatabaseMaintenance","Backup, repair, or optimize has failed.  Your database has not been altered.")+"\r\n";
				result+=Lan.g("FormDatabaseMaintenance","Please call support for help, a manual backup of your data must be made before trying to fix your database.")+"\r\n";
			}
			Cursor=Cursors.Default;
			SaveLogToFile(result);
			MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(result);
			msgBoxCP.Show();//Let this window be non-modal so that they can keep it open while they fix their problems.
		}

		private void butApptProcs_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will fix procedure descriptions in the Appt module that aren't correctly showing tooth numbers.\r\nContinue?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			Appointments.UpdateProcDescriptForAppts(Appointments.GetForPeriod(DateTime.Now.Date.AddMonths(-6),DateTime.MaxValue.AddDays(-10)).ToList());
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Done. Please refresh Appt module to see the changes.");
		}

		private void butSpecChar_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This is only used if your mobile synch or middle tier is failing.  This cannot be undone.  Do you wish to continue?")) {
				return;
			}
			InputBox box=new InputBox("In our online manual, on the database maintenance page, look for the password and enter it below.");
			if(box.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(box.textResult.Text!="fix") {
				MessageBox.Show("Wrong password.");
				return;
			}
			DatabaseMaintenances.FixSpecialCharacters();
			MsgBox.Show(this,"Done.");
			_isCacheInvalid=true;//Definitions are cached and could have been changed from above DBM.
		}

		private void butInnoDB_Click(object sender,EventArgs e) {
			FormInnoDb form=new FormInnoDb();
			form.ShowDialog();
		}

		private void butTokens_Click(object sender,EventArgs e) {
			FormXchargeTokenTool FormXCT=new FormXchargeTokenTool();
			FormXCT.ShowDialog();
		}

		private void butRemoveNulls_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will replace ALL null strings in your database with empty strings.  This cannot be undone.  Do you wish to continue?")) {
				return;
			}
			MessageBox.Show(Lan.g(this,"Number of null strings replaced with empty strings")+": "+DatabaseMaintenances.MySqlRemoveNullStrings());
			_isCacheInvalid=true;//The above DBM could have potentially changed cached tables. 
		}

		private void butEtrans_Click(object sender,EventArgs e) {
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				MsgBox.Show(this,"Tool does not currently support Oracle.  Please call support to see if you need this fix.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will clear out etrans message text entries over a year old.  An automatic backup of the database will be created before deleting any entries.  This process may take a while to run depending on the size of your database.  Continue?")) {
				return;
			}
#if !DEBUG
			if(!Shared.MakeABackup(BackupLocation.DatabaseMaintenanceTool)) {
				MsgBox.Show(this,"Etrans message text entries were not altered.  Please try again.");
				return;
			}
#endif
			DatabaseMaintenances.ClearOldEtransMessageText();
			MsgBox.Show(this,"Etrans message text entries over a year old removed");
		}

		private void butActiveTPs_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			List<Procedure> listTpTpiProcs=DatabaseMaintenances.GetProcsNoActiveTp();
			Cursor=Cursors.Default;
			if(listTpTpiProcs.Count==0) {
				MsgBox.Show(this,"Done");
				return;
			}
			int numTPs=listTpTpiProcs.Select(x => x.PatNum).Distinct().ToList().Count;
			int numTPAs=listTpTpiProcs.Count;
			TimeSpan estRuntime=TimeSpan.FromSeconds((numTPs+numTPAs)*0.001d);
			//the factor 0.001 was determined by running tests on a large db
			//212631 TPAs and 30000 TPs were inserted in 225 seconds
			//225/(212631+30000)=0.0009273341 seconds per inserted row (rounded up to 0.001 seconds)
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"From your database size we estimate that this could take "+(estRuntime.Minutes+1)+" minutes to create "
				+numTPs+" treatment plans for "+numTPAs+" procedures if running form the server.\r\nDo you wish to continue?"))
			{
				return;
			}
			Cursor=Cursors.WaitCursor;
			string msg=DatabaseMaintenances.CreateMissingActiveTPs(listTpTpiProcs);
			Cursor=Cursors.Default;
			if(string.IsNullOrEmpty(msg)) {
				msg="Done";
			}
			MsgBox.Show(this,msg);
		}

		private void butRawEmails_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo
				,"This tool is only necessary to run if utilizing the email inbox feature.\r\n"
				+"Run this tool if email messages are encoded in the Chart progress notes, \r\n"
				+"or if the emailmessage table has grown to a large size.\r\n"
				+"This will decode any encoded clear text emails and will remove unused attachment content.\r\n\r\n"
				+"This tool could take a long time to finish, do you wish to continue?"))
			{
				return;
			}
			//Create a new thread that will do the work while the progress window is on the main thread (takes a while even if no clean up needed) 
			//so the user know work is being done.
			string results="";
			ODProgress.ShowAction(() => results=DatabaseMaintenances.CleanUpRawEmails(),
				actionException:ex => {
					results=Lan.g(this,"There was an error cleaning up email bloat:")+"\r\n"+ex.Message;
				},
				eventType:typeof(DatabaseMaintEvent),
				odEventType:ODEventType.DatabaseMaint);
			MessageBox.Show(results);
		}

		private void butEmailAttaches_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo
				,"This tool is only necessary to run if utilizing the email feature.\r\n"
				+"Run this tool if there are files that start with 'In_' or 'Out_' within the AtoZ EmailAttachments folder.  "
				+"The issue is evident when trying to view an attachment and a file not found error occurs.\r\n\r\n"
				+"This tool could take a long time to finish, do you wish to continue?"))
			{
				return;
			}
			string results="";
			ODProgress.ShowAction(() => results=DatabaseMaintenances.CleanUpAttachmentsRootDirectiory(),
				actionException: ex => {
					results=Lan.g(this,"There was an error cleaning up email attachments:")+"\r\n"+ex.Message;
				},
				eventType: typeof(DatabaseMaintEvent),
				odEventType: ODEventType.DatabaseMaint);
			MsgBoxCopyPaste msgBoxCopyPaste=new MsgBoxCopyPaste(results);
			msgBoxCopyPaste.Show();
		}

		private void butRecalcEst_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo
				,"This tool will mimic what happens when you click OK in the procedure edit window.  "
				+"The tool will identify patients with at least one estimate which belongs to a dropped insurance plan.  "
				+"For each such patient, estimates will be recalculated for current plans, and  "
				+"for plans which have been dropped, estimates associated to the dropped plans will be deleted.\r\n"
				+"This tool could take a long time to finish, do you wish to continue?"))
			{
				return;
			}
			//Create a new thread that will do the work while the progress window is on the main thread (takes a while even if no clean up needed) 
			//so the user know work is being done.
			ODProgress.ShowAction(() => DatabaseMaintenances.RecalcEstimates(Procedures.GetProcsWithOldEstimates()));
		}
		
		private void butPayPlanPayments_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo
				,"You are running a tool to detach patient payments from insurance payment plans and detach insurance payments from patient payment plans.  "
				+"The payments will still exist, and because they will now be reflected in the account instead of the payment plan, historical and "
				+"current account balances may change.  Proceed?"))
			{
				return;
			}
			//Create a new thread that will do the work while the progress window is on the main thread (takes a while even if no clean up needed) 
			//so the user know work is being done.
			string results="";
			ODProgress.ShowAction(() => results=DatabaseMaintenances.DetachInvalidPaymentPlanPayments());
			MsgBoxCopyPaste msgBoxCopyPaste=new MsgBoxCopyPaste(results);
			msgBoxCopyPaste.ShowInTaskbar=true;
			msgBoxCopyPaste.Text=Lan.g(this,"Payments Fixed");
			msgBoxCopyPaste.Show();
		}

		private void butFamilyBalance_Click(object sender,EventArgs e) {
			InputBox inputbox=new InputBox("Please enter password");
			inputbox.setTitle("Family Balancer");
			inputbox.textResult.PasswordChar='*';
			inputbox.ShowDialog();
			if(inputbox.DialogResult!=DialogResult.OK) {
				return;
			}
			if(inputbox.textResult.Text!="ConversionsDepartment") {
				MsgBox.Show(this,"Wrong password");
				return;
			}
			if(Security.IsAuthorized(Permissions.SecurityAdmin)) {
				FormFamilyBalancer form=new FormFamilyBalancer();
				form.ShowDialog();
			}
		}

		#endregion

		private void butTemp_Click(object sender,EventArgs e) {
			FormDatabaseMaintTemp form=new FormDatabaseMaintTemp();
			form.ShowDialog();
		}

		private void Run(ODGrid grid,DbmMode modeCur) {
			ToggleUI(true);//Turn off all UI buttons except the Stop DBM button
			_isCancelled=false;
			Cursor=Cursors.WaitCursor;
			if(grid.ListGridRows.Count > 0 && grid.ListGridColumns.Count < 3) {//Enforce the requirement of having the Results column as the third column.
				MsgBox.Show(this,"Must have at least three columns in the grid.");
				return;
			}
			int colresults=2;
			if(grid==gridOld && checkShowHidden.Checked) {
				colresults=3;//There is an extra "Hidden" column to account for when setting the "Results" column.
			}
			//Clear out the result column for all rows before every "run"
			for(int i=0;i<grid.ListGridRows.Count;i++) {
				grid.ListGridRows[i].Cells[colresults].Text="";//Don't use UpdateResultTextForRow here because users will see the rows clearing out one by one.
			}
			bool verbose=checkShow.Checked;
			StringBuilder logText=new StringBuilder();
			//Create a window that will stay open until the thread doing the work is complete
			ODTuple<string,bool> tableCheckResult=null;
			ODProgress.ShowAction(() => tableCheckResult=DatabaseMaintenances.MySQLTables(verbose,modeCur),
				eventType:typeof(DatabaseMaintEvent),
				odEventType:ODEventType.DatabaseMaint);
			logText.Append(tableCheckResult.Item1);
			//No database maintenance methods should be run unless this passes.
			if(!tableCheckResult.Item2) {
				Cursor=Cursors.Default;
				MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(tableCheckResult.Item1);//tableCheckResult is already translated.
				msgBoxCP.Show();//Let this window be non-modal so that they can keep it open while they fix their problems.
				return;
			}
			if(grid.SelectedIndices.Length < 1) {
				//No rows are selected so the user wants to run all checks.
				grid.SetSelected(true);
			}
			int[] selectedIndices=grid.SelectedIndices;
			int selectedIndex=-1;
			//Create worker thread to run DBMs. This allows the user to stop running DBM without waiting for all methods to finish
			_threadRunDBM=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				for(int i=0;i<selectedIndices.Length;i++) {
					selectedIndex=selectedIndices[i];
					MethodInfo method=(MethodInfo)grid.ListGridRows[selectedIndices[i]].Tag;
					ScrollToBottom(grid,selectedIndices[i]);
					UpdateResultTextForRow(grid,selectedIndices[i],Lan.g("FormDatabaseMaintenance","Running")+"...");
					string result=RunMethod(method,modeCur);
					string status="";
					if(result=="") {//Only possible if running a check / fix in non-verbose mode and nothing happened or needs to happen.
						status=Lan.g("FormDatabaseMaintenance","Done.  No maintenance needed.");
					}
					UpdateResultTextForRow(grid,selectedIndices[i],result+status);
					logText.Append(result);
					//Check flag to see if user wants to stop DBM
					if(_isCancelled) {
						break;
					}
				}
				ToggleUI(false);
				grid.SetSelected(selectedIndices,true);//Reselect all rows that were originally selected.
			}));
			_threadRunDBM.AddExitHandler((ex) => SaveLogToFile(logText.ToString()));
			_threadRunDBM.AddExceptionHandler(ex => this.InvokeIfRequired(() => {
				FriendlyException.Show("Error during database maintenance.",ex);
				if(selectedIndex>=0) {
					UpdateResultTextForRow(grid,selectedIndex,Lan.g(this,"ERROR: ")+ex.Message);
				}
				logText.Append(Lan.g(this,"ERROR: ")+ex.Message);
				ToggleUI(false);
				grid.SetSelected(selectedIndices,true);//Reselect all rows that were originally selected.
			}));
			_threadRunDBM.Name="RunDBMThread";
			_threadRunDBM.Start();
			Cursor=Cursors.Default;
		} 

		///<summary>Runs a single DBM method.  Updates the DateLastRun column in the database for the method passed in if modeCur is set to Fix.</summary>
		private string RunMethod(MethodInfo method,DbmMode modeCur) {
			List<object> parameters=GetParametersForMethod(method,modeCur);
			return RunMethod(method,parameters,modeCur);
		}

		///<summary>Runs a single DBM method.  Updates the DateLastRun column in the database for the method passed in if modeCur is set to Fix.</summary>
		private string RunMethod(MethodInfo method,List<object> parameters,DbmMode modeCur) {
			string result="";
			try {
				result=(string)method.Invoke(null,parameters.ToArray());
				if(modeCur==DbmMode.Fix) {
					DatabaseMaintenances.UpdateDateLastRun(method.Name);
				}
			}
			catch(Exception ex) {
				if(ex.InnerException!=null) {
					ExceptionDispatchInfo.Capture(ex.InnerException).Throw();//This preserves the stack trace of the InnerException.
				}
				throw;
			}
			return result;
		}

		///<summary>Runs the DBM method passed in and displays the results in a non-modal MsgBoxCopyPaste window.
		///Does nothing if the DBM method passed in is not flagged for HasBreakDown</summary>
		private void RunMethodBreakDown(MethodInfo method) {
			if(!DatabaseMaintenances.MethodHasBreakDown(method)) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			string result=RunMethod(method,DbmMode.Breakdown);
			Cursor=Cursors.Default;
			if(result=="") {
				result=Lan.g("FormDatabaseMaintenance","Done.  No maintenance needed.");
			}
			SaveLogToFile(method.Name+":\r\n"+result);
			//Show the result of the dbm method in a simple copy paste msg box.
			MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(result);
			msgBoxCP.Show();//Let this window be non-modal so that they can keep it open while they fix their problems.
		}

		///<summary>Returns a list of parameters for the corresponding DBM method.  The order of these parameters is critical.</summary>
		private List<object> GetParametersForMethod(MethodInfo method,DbmMode modeCur) {
			long patNum=0;
			DbmMethodAttr methodAttributes=(DbmMethodAttr)Attribute.GetCustomAttribute(method,typeof(DbmMethodAttr));
			//There are optional paramaters available to some methods and adding them in the following order is very important.
			//We always send verbose and modeCur into all DBM methods first.
			List<object> parameters=new List<object>() { checkShow.Checked,modeCur };
			//Followed by an optional PatNum for patient specific DBM methods.
			if(methodAttributes.HasPatNum) {
				parameters.Add(patNum);
			}
			return parameters;
		}

		///<summary>Tries to log the text passed in to a centralized DBM log.  Displays an error message to the user if anything goes wrong.
		///Always sets the current Cursor state back to Cursors.Default once finished.</summary>
		private void SaveLogToFile(string logText) {
			this.InvokeIfNotDisposed(() => {
				try {
					DatabaseMaintenances.SaveLogToFile(logText);
				}
				catch(Exception ex) {
					Cursor=Cursors.Default;
					MessageBox.Show(ex.Message);
				}
				Cursor=Cursors.Default;
			});
		}

		///<summary>Helper function to make thread-safe UI calls to ODgrid.ScrollToIndexBottom</summary>
		private void ScrollToBottom(ODGrid grid,int index) {
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate () { ScrollToBottom(grid,index); });
				return;
			}
			grid.ScrollToIndexBottom(index);
		}

		/// <summary>Updates the result column for the specified row in gridMain with the text passed in.</summary>
		private void UpdateResultTextForRow(ODGrid grid,int index,string text) {
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate () { UpdateResultTextForRow(grid,index,text); });
				return;
			}
			int colresults=2;
			if(grid==gridOld && checkShowHidden.Checked) {
				colresults=3;//There is an extra "Hidden" column to account for when setting the "Results" column.
			}
			grid.BeginUpdate();
			grid.ListGridRows[index].Cells[colresults].Text=text;
			grid.EndUpdate();
			Application.DoEvents();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			if(_dateTimeLastRun==DateTime.MinValue) {
				_dateTimeLastRun=DateTime.Now;
			}
			StringBuilder strB=new StringBuilder();
			strB.Append(_dateTimeLastRun.ToString());
			strB.Append('-',65);
			strB.AppendLine();//New line.
			if(gridMain.SelectedIndices.Length < 1) {
				//No rows are selected so the user wants to run all checks.
				gridMain.SetSelected(true);
			}
			int[] selectedIndices=gridMain.SelectedIndices;
			for(int i=0;i<selectedIndices.Length;i++) {
				string resultText=gridMain.ListGridRows[selectedIndices[i]].Cells[2].Text;
				if(!String.IsNullOrEmpty(resultText) && resultText!="Done.  No maintenance needed.") {
					strB.Append(gridMain.ListGridRows[selectedIndices[i]].Cells[0].Text+"\r\n");
					strB.Append("---"+gridMain.ListGridRows[selectedIndices[i]].Cells[2].Text+"\r\n");
					strB.AppendLine();
				}
			}
			strB.AppendLine(Lan.g("FormDatabaseMaintenance","Done"));
			LogTextPrint=strB.ToString();
			PrinterL.TryPrintOrDebugClassicPreview(pd2_PrintPage,Lan.g(this,"Database Maintenance log printed"),new Margins(40,50,50,60),0);
		}

		///<summary>Turn certain UI elements on or off depending on if DBM is running.</summary>
		private void ToggleUI(bool isRunningDbm=false) {
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate () { ToggleUI(isRunningDbm); });
				return;
			}
			if(isRunningDbm) {
				butCheck.Enabled=false;
				butCheckOld.Enabled=false;
				butFix.Enabled=false;
				butFixOld.Enabled=false;
				butPrint.Enabled=false;
				butNone.Enabled=false;
				butClose.Enabled=false;
				butStopDBM.Enabled=true;
				butStopDBMOld.Enabled=true;
				tabTools.Enabled=false;
				checkShow.Enabled=false;
				checkShowHidden.Enabled=false;
				ControlBox=false;//We do NOT want the user to click the X button. They must click the stop button if DBM is running.
			}
			else {
				butCheck.Enabled=true;
				butCheckOld.Enabled=true;
				butFix.Enabled=true;
				butFixOld.Enabled=true;
				butPrint.Enabled=true;
				butNone.Enabled=true;
				butClose.Enabled=true;
				butStopDBM.Enabled=false;
				butStopDBMOld.Enabled=false;
				tabTools.Enabled=true;
				checkShow.Enabled=true;
				checkShowHidden.Enabled=true;
				ControlBox=true;
			}
		}

		private void pd2_PrintPage(object sender,PrintPageEventArgs ev) {//raised for each page to be printed.
			int charsOnPage=0;
			int linesPerPage=0;
			Font font=new Font("Courier New",10);
			ev.Graphics.MeasureString(LogTextPrint,font,ev.MarginBounds.Size,StringFormat.GenericTypographic,out charsOnPage,out linesPerPage);
			ev.Graphics.DrawString(LogTextPrint,font,Brushes.Black,ev.MarginBounds,StringFormat.GenericTypographic);
			LogTextPrint=LogTextPrint.Substring(charsOnPage);
			ev.HasMorePages=(LogTextPrint.Length > 0);
		}
		
		private void checkShowHidden_CheckedChanged(object sender,EventArgs e) {
			FillGridOld();
		}

		private void butCheck_Click(object sender,System.EventArgs e) {
			Run(gridMain,DbmMode.Check);
		}

		private void butCheckOld_Click(object sender,EventArgs e) {
			Run(gridOld,DbmMode.Check);
		}

		private void butFix_Click(object sender,EventArgs e) {
			Fix();
		}

		private void butFixOld_Click(object sender,EventArgs e) {
			Fix(isOld:true);
		}

		private void butStopDBMOld_Click(object sender,EventArgs e) {
			_isCancelled=true;
			Cursor=Cursors.WaitCursor;
		}

		private void butStopDBM_Click(object sender,EventArgs e) {
			_isCancelled=true;
			Cursor=Cursors.WaitCursor;
		}

		private void Fix(bool isOld=false) {
			List<Computer> runningComps=Computers.GetRunningComputers();
			if(runningComps.Count>50) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"WARNING!\r\nMore than 50 workstations are connected to this database. "
					+"Running DBM may cause severe network slowness. "
					+"We recommend running this tool when fewer users are connected (possibly after working hours). \r\n\r\n"
					+"Continue?")) {
					return;
				}
			}
			if(isOld) {
				Run(gridOld,DbmMode.Fix);
			}
			else {
				Run(gridMain,DbmMode.Fix);
			}
			_isCacheInvalid=true;//Flag cache to be invalidated on closing.  Some DBM fixes alter cached tables.
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			Close();
		}

		private void FormDatabaseMaintenance_FormClosing(object sender,FormClosingEventArgs e) {
			if(_isCacheInvalid) {
				//Invalidate all cached tables.  DBM could have touched anything so blast them all.
				//Failure to invalidate cache can cause UEs in the main program.
				Action actionCloseProgress=ODProgress.Show(odEventType:ODEventType.Cache);
				DataValid.SetInvalid(Cache.GetAllCachedInvalidTypes().ToArray());
				actionCloseProgress();
			}
		}
	}


}
