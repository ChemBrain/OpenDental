namespace OpenDental{
	partial class FormSupportStatus {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSupportStatus));
			this.butClose = new OpenDental.UI.Button();
			this.textRegKey = new System.Windows.Forms.TextBox();
			this.labelRegKey = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.labelStatusValue = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(283, 181);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// textRegKey
			// 
			this.textRegKey.Location = new System.Drawing.Point(162, 51);
			this.textRegKey.Name = "textRegKey";
			this.textRegKey.ReadOnly = true;
			this.textRegKey.Size = new System.Drawing.Size(159, 20);
			this.textRegKey.TabIndex = 4;
			// 
			// labelRegKey
			// 
			this.labelRegKey.Location = new System.Drawing.Point(53, 50);
			this.labelRegKey.Name = "labelRegKey";
			this.labelRegKey.Size = new System.Drawing.Size(103, 20);
			this.labelRegKey.TabIndex = 5;
			this.labelRegKey.Text = "Registration Key";
			this.labelRegKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelStatus
			// 
			this.labelStatus.Location = new System.Drawing.Point(53, 108);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(100, 20);
			this.labelStatus.TabIndex = 6;
			this.labelStatus.Text = "Support Status:";
			this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelStatusValue
			// 
			this.labelStatusValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelStatusValue.ForeColor = System.Drawing.Color.Black;
			this.labelStatusValue.Location = new System.Drawing.Point(159, 108);
			this.labelStatusValue.Name = "labelStatusValue";
			this.labelStatusValue.Size = new System.Drawing.Size(199, 20);
			this.labelStatusValue.TabIndex = 7;
			this.labelStatusValue.Text = "Loading...";
			this.labelStatusValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormSupportStatus
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(370, 217);
			this.Controls.Add(this.labelStatusValue);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.labelRegKey);
			this.Controls.Add(this.textRegKey);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSupportStatus";
			this.Text = "Support Status";
			this.Load += new System.EventHandler(this.FormSupportStatus_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.TextBox textRegKey;
		private System.Windows.Forms.Label labelRegKey;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Label labelStatusValue;
	}
}