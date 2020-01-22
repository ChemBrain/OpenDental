namespace OpenDental{
	partial class FormClaimAttachmentItemEdit {
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
			this.comboImageType = new OpenDental.UI.ComboBoxPlus();
			this.labelImageType = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBoxOrientationType = new System.Windows.Forms.GroupBox();
			this.labelRight = new System.Windows.Forms.Label();
			this.labelLeft = new System.Windows.Forms.Label();
			this.radioButtonRight = new System.Windows.Forms.RadioButton();
			this.radioButtonLeft = new System.Windows.Forms.RadioButton();
			this.textDateCreated = new OpenDental.ValidDate();
			this.labelDateTimeCreate = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.textFileName = new System.Windows.Forms.TextBox();
			this.groupBoxOrientationType.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboImageType
			// 
			this.comboImageType.Location = new System.Drawing.Point(139, 94);
			this.comboImageType.Name = "comboImageType";
			this.comboImageType.Size = new System.Drawing.Size(121, 21);
			this.comboImageType.TabIndex = 2;
			this.comboImageType.SelectionChangeCommitted += new System.EventHandler(this.ComboImageType_SelectionChangeCommitted_1);
			// 
			// labelImageType
			// 
			this.labelImageType.Location = new System.Drawing.Point(54, 97);
			this.labelImageType.Name = "labelImageType";
			this.labelImageType.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.labelImageType.Size = new System.Drawing.Size(84, 15);
			this.labelImageType.TabIndex = 1;
			this.labelImageType.Text = "Image Type";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(54, 44);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.label1.Size = new System.Drawing.Size(84, 15);
			this.label1.TabIndex = 2;
			this.label1.Text = "File Name";
			// 
			// groupBoxOrientationType
			// 
			this.groupBoxOrientationType.Controls.Add(this.labelRight);
			this.groupBoxOrientationType.Controls.Add(this.labelLeft);
			this.groupBoxOrientationType.Controls.Add(this.radioButtonRight);
			this.groupBoxOrientationType.Controls.Add(this.radioButtonLeft);
			this.groupBoxOrientationType.Location = new System.Drawing.Point(37, 121);
			this.groupBoxOrientationType.Name = "groupBoxOrientationType";
			this.groupBoxOrientationType.Size = new System.Drawing.Size(324, 128);
			this.groupBoxOrientationType.TabIndex = 4;
			this.groupBoxOrientationType.TabStop = false;
			this.groupBoxOrientationType.Text = "Image Orientation Type";
			// 
			// labelRight
			// 
			this.labelRight.Location = new System.Drawing.Point(102, 16);
			this.labelRight.Name = "labelRight";
			this.labelRight.Size = new System.Drawing.Size(216, 53);
			this.labelRight.TabIndex = 8;
			this.labelRight.Text = "Indicates that the left hand side of the image is the patient\'s right hand side. " +
    "A value of \"RIGHT\" is typical.";
			// 
			// labelLeft
			// 
			this.labelLeft.Location = new System.Drawing.Point(102, 72);
			this.labelLeft.Name = "labelLeft";
			this.labelLeft.Size = new System.Drawing.Size(216, 41);
			this.labelLeft.TabIndex = 7;
			this.labelLeft.Text = "Indicates that the left hand side of the image is the patient\'s left hand side.";
			// 
			// radioButtonRight
			// 
			this.radioButtonRight.Checked = true;
			this.radioButtonRight.Location = new System.Drawing.Point(29, 16);
			this.radioButtonRight.Name = "radioButtonRight";
			this.radioButtonRight.Size = new System.Drawing.Size(59, 17);
			this.radioButtonRight.TabIndex = 4;
			this.radioButtonRight.TabStop = true;
			this.radioButtonRight.Text = "RIGHT";
			this.radioButtonRight.UseVisualStyleBackColor = true;
			// 
			// radioButtonLeft
			// 
			this.radioButtonLeft.Location = new System.Drawing.Point(29, 72);
			this.radioButtonLeft.Name = "radioButtonLeft";
			this.radioButtonLeft.Size = new System.Drawing.Size(57, 17);
			this.radioButtonLeft.TabIndex = 3;
			this.radioButtonLeft.TabStop = true;
			this.radioButtonLeft.Text = "LEFT";
			this.radioButtonLeft.UseVisualStyleBackColor = true;
			// 
			// textDateCreated
			// 
			this.textDateCreated.Location = new System.Drawing.Point(139, 68);
			this.textDateCreated.Name = "textDateCreated";
			this.textDateCreated.Size = new System.Drawing.Size(121, 20);
			this.textDateCreated.TabIndex = 1;
			// 
			// labelDateTimeCreate
			// 
			this.labelDateTimeCreate.Location = new System.Drawing.Point(54, 71);
			this.labelDateTimeCreate.Name = "labelDateTimeCreate";
			this.labelDateTimeCreate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.labelDateTimeCreate.Size = new System.Drawing.Size(84, 15);
			this.labelDateTimeCreate.TabIndex = 6;
			this.labelDateTimeCreate.Text = "Date Created";
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(230, 275);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 5;
			this.butOK.Text = "OK";
			this.butOK.UseVisualStyleBackColor = true;
			this.butOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(311, 275);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 6;
			this.butCancel.Text = "Cancel";
			this.butCancel.UseVisualStyleBackColor = true;
			this.butCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// textFileName
			// 
			this.textFileName.Location = new System.Drawing.Point(139, 41);
			this.textFileName.Name = "textFileName";
			this.textFileName.Size = new System.Drawing.Size(121, 20);
			this.textFileName.TabIndex = 0;
			// 
			// FormClaimAttachmentItemEdit
			// 
			this.ClientSize = new System.Drawing.Size(398, 311);
			this.Controls.Add(this.textFileName);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.labelDateTimeCreate);
			this.Controls.Add(this.textDateCreated);
			this.Controls.Add(this.groupBoxOrientationType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelImageType);
			this.Controls.Add(this.comboImageType);
			this.MaximumSize = new System.Drawing.Size(414, 350);
			this.Name = "FormClaimAttachmentItemEdit";
			this.Text = "Image Info";
			this.groupBoxOrientationType.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		
		private UI.ComboBoxPlus comboImageType;
		private System.Windows.Forms.Label labelImageType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBoxOrientationType;
		private System.Windows.Forms.RadioButton radioButtonRight;
		private System.Windows.Forms.RadioButton radioButtonLeft;
		private System.Windows.Forms.Label labelLeft;
		private System.Windows.Forms.Label labelRight;
		private ValidDate textDateCreated;
		private System.Windows.Forms.Label labelDateTimeCreate;
		private UI.Button butOK;
		private UI.Button butCancel;
		private System.Windows.Forms.TextBox textFileName;
	}
}