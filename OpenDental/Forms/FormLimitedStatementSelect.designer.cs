namespace OpenDental{
	partial class FormLimitedStatementSelect {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLimitedStatementSelect));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butNone = new OpenDental.UI.Button();
			this.butAll = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.odDateRangePicker = new OpenDental.UI.ODDateRangePicker();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboBoxMultiTransTypes = new OpenDental.UI.ComboBoxMulti();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(681, 535);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(681, 565);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butNone
			// 
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNone.Location = new System.Drawing.Point(681, 309);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(75, 26);
			this.butNone.TabIndex = 148;
			this.butNone.Text = "None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// butAll
			// 
			this.butAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAll.Location = new System.Drawing.Point(681, 277);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(75, 26);
			this.butAll.TabIndex = 147;
			this.butAll.Text = "All";
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.EditableEnterMovesDown = false;
			this.gridMain.Location = new System.Drawing.Point(13, 84);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(661, 505);
			this.gridMain.TabIndex = 146;
			this.gridMain.Title = "Limited Statement Items";
			this.gridMain.TranslationName = "TableLimitedStatementItems";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(691, 18);
			this.label1.TabIndex = 145;
			this.label1.Text = "Select procedures, payments, adjustments, claims, or payment plan debits to inclu" +
    "de on the limited statement";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// odDateRangePicker
			// 
			this.odDateRangePicker.BackColor = System.Drawing.Color.Transparent;
			this.odDateRangePicker.DefaultDateTimeFrom = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
			this.odDateRangePicker.DefaultDateTimeTo = new System.DateTime(2019, 1, 11, 0, 0, 0, 0);
			this.odDateRangePicker.Location = new System.Drawing.Point(4, 16);
			this.odDateRangePicker.MinimumSize = new System.Drawing.Size(453, 22);
			this.odDateRangePicker.Name = "odDateRangePicker";
			this.odDateRangePicker.Size = new System.Drawing.Size(453, 24);
			this.odDateRangePicker.TabIndex = 149;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.comboBoxMultiTransTypes);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.odDateRangePicker);
			this.groupBox1.Location = new System.Drawing.Point(13, 23);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(743, 55);
			this.groupBox1.TabIndex = 150;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Filters";
			// 
			// comboBoxMultiTransTypes
			// 
			this.comboBoxMultiTransTypes.ArraySelectedIndices = new int[0];
			this.comboBoxMultiTransTypes.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiTransTypes.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiTransTypes.Items")));
			this.comboBoxMultiTransTypes.Location = new System.Drawing.Point(549, 18);
			this.comboBoxMultiTransTypes.Name = "comboBoxMultiTransTypes";
			this.comboBoxMultiTransTypes.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiTransTypes.SelectedIndices")));
			this.comboBoxMultiTransTypes.Size = new System.Drawing.Size(120, 21);
			this.comboBoxMultiTransTypes.TabIndex = 152;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(428, 18);
			this.label2.Name = "label2";
			this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label2.Size = new System.Drawing.Size(115, 21);
			this.label2.TabIndex = 151;
			this.label2.Text = "Transaction types:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormLimitedStatementSelect
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(768, 600);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.butAll);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(784, 447);
			this.Name = "FormLimitedStatementSelect";
			this.Text = "Limited Statement Select";
			this.Load += new System.EventHandler(this.FormLimitedStatementSelect_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butNone;
		private UI.Button butAll;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.Label label1;
		private UI.ODDateRangePicker odDateRangePicker;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private UI.ComboBoxMulti comboBoxMultiTransTypes;
	}
}