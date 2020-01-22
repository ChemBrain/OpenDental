namespace OpenDental{
	partial class FormRpServiceDateView {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpServiceDateView));
			this.butClose = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butSavePDFToImages = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(900, 359);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butPrint
			// 
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(12, 357);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 26);
			this.butPrint.TabIndex = 4;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridMain.Size = new System.Drawing.Size(963, 339);
			this.gridMain.TabIndex = 29;
			this.gridMain.Title = "Service View";
			this.gridMain.TranslationName = "TableServiceView";
			// 
			// butSavePDFToImages
			// 
			this.butSavePDFToImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSavePDFToImages.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSavePDFToImages.Location = new System.Drawing.Point(107, 357);
			this.butSavePDFToImages.Name = "butSavePDFToImages";
			this.butSavePDFToImages.Size = new System.Drawing.Size(88, 26);
			this.butSavePDFToImages.TabIndex = 84;
			this.butSavePDFToImages.TabStop = false;
			this.butSavePDFToImages.Text = "Save to Images";
			this.butSavePDFToImages.Click += new System.EventHandler(this.butSavePDFToImages_Click);
			// 
			// FormRpServiceDateView
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(987, 395);
			this.Controls.Add(this.butSavePDFToImages);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1003, 434);
			this.Name = "FormRpServiceDateView";
			this.Text = "Service Date View";
			this.Load += new System.EventHandler(this.FormRpServiceDate_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private OpenDental.UI.Button butClose;
		private UI.Button butPrint;
		private UI.ODGrid gridMain;
		private UI.Button butSavePDFToImages;
	}
}