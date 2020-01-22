namespace OpenDental{
	partial class FormJobEdit {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobEdit));
			this.userControlJobEdit = new OpenDental.InternalTools.Job_Manager.UserControlJobEdit();
			this.userControlQueryEdit = new OpenDental.InternalTools.Job_Manager.UserControlQueryEdit();
			this.SuspendLayout();
			// 
			// userControlJobEdit
			// 
			this.userControlJobEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlJobEdit.Enabled = false;
			this.userControlJobEdit.IsOverride = false;
			this.userControlJobEdit.Location = new System.Drawing.Point(0, 0);
			this.userControlJobEdit.Name = "userControlJobEdit";
			this.userControlJobEdit.Size = new System.Drawing.Size(1213, 885);
			this.userControlJobEdit.TabIndex = 4;
			this.userControlJobEdit.SaveClick += new System.EventHandler(this.userControlJobEdit_SaveClick);
			// 
			// userControlQueryEdit
			// 
			this.userControlQueryEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlQueryEdit.IsOverride = false;
			this.userControlQueryEdit.Location = new System.Drawing.Point(0, 0);
			this.userControlQueryEdit.Name = "userControlQueryEdit";
			this.userControlQueryEdit.Size = new System.Drawing.Size(1213, 885);
			this.userControlQueryEdit.TabIndex = 5;
			this.userControlQueryEdit.Visible = false;
			this.userControlQueryEdit.SaveClick += new System.EventHandler(this.userControlQueryEdit_SaveClick);
			// 
			// FormJobEdit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1213, 885);
			this.Controls.Add(this.userControlJobEdit);
			this.Controls.Add(this.userControlQueryEdit);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormJobEdit";
			this.Text = "Job Edit";
			this.Activated += new System.EventHandler(this.FormJobEdit_Activated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJobEdit_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion
		private InternalTools.Job_Manager.UserControlJobEdit userControlJobEdit;
		private InternalTools.Job_Manager.UserControlQueryEdit userControlQueryEdit;
	}
}