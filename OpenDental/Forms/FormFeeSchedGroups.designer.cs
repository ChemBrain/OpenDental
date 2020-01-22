namespace OpenDental{
	partial class FormFeeSchedGroups {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeeSchedGroups));
			this.butClose = new OpenDental.UI.Button();
			this.gridGroups = new OpenDental.UI.ODGrid();
			this.gridClinics = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(475, 314);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridGroups
			// 
			this.gridGroups.AllowSortingByColumn = true;
			this.gridGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridGroups.Location = new System.Drawing.Point(11, 12);
			this.gridGroups.Name = "gridGroups";
			this.gridGroups.Size = new System.Drawing.Size(262, 289);
			this.gridGroups.TabIndex = 4;
			this.gridGroups.Title = "Fee Schedule Groups";
			this.gridGroups.TranslationName = "Table Fee Schedule Groups";
			this.gridGroups.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridGroups_CellDoubleClick);
			this.gridGroups.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridGroups_CellClick);
			// 
			// gridClinics
			// 
			this.gridClinics.AllowSortingByColumn = true;
			this.gridClinics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridClinics.Location = new System.Drawing.Point(289, 12);
			this.gridClinics.Name = "gridClinics";
			this.gridClinics.Size = new System.Drawing.Size(262, 289);
			this.gridClinics.TabIndex = 5;
			this.gridClinics.Title = "Clinics in Group";
			this.gridClinics.TranslationName = "Table Clinics";
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Image = ((System.Drawing.Image)(resources.GetObject("butAdd.Image")));
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(10, 314);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(93, 23);
			this.butAdd.TabIndex = 220;
			this.butAdd.Text = "&Add Group";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// FormFeeSchedGroups
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(560, 349);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.gridClinics);
			this.Controls.Add(this.gridGroups);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(576, 388);
			this.Name = "FormFeeSchedGroups";
			this.Text = "Fee Schedule Groups";
			this.Load += new System.EventHandler(this.FormFeeSchedGroups_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridGroups;
		private UI.ODGrid gridClinics;
		private UI.Button butAdd;
	}
}