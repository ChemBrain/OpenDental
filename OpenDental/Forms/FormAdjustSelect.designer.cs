namespace OpenDental{
	partial class FormAdjustSelect {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdjustSelect));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBoxSum = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelAmtEnd = new System.Windows.Forms.Label();
			this.labelCurSplitAmt = new System.Windows.Forms.Label();
			this.labelAmtAvail = new System.Windows.Forms.Label();
			this.labelAmtUsed = new System.Windows.Forms.Label();
			this.labelAmtOriginal = new System.Windows.Forms.Label();
			this.gridAdjusts = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBoxSum.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(341, 393);
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
			this.butCancel.Location = new System.Drawing.Point(422, 393);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupBoxSum
			// 
			this.groupBoxSum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxSum.Controls.Add(this.label9);
			this.groupBoxSum.Controls.Add(this.label12);
			this.groupBoxSum.Controls.Add(this.label6);
			this.groupBoxSum.Controls.Add(this.label3);
			this.groupBoxSum.Controls.Add(this.label2);
			this.groupBoxSum.Controls.Add(this.labelAmtEnd);
			this.groupBoxSum.Controls.Add(this.labelCurSplitAmt);
			this.groupBoxSum.Controls.Add(this.labelAmtAvail);
			this.groupBoxSum.Controls.Add(this.labelAmtUsed);
			this.groupBoxSum.Controls.Add(this.labelAmtOriginal);
			this.groupBoxSum.Location = new System.Drawing.Point(341, 51);
			this.groupBoxSum.Name = "groupBoxSum";
			this.groupBoxSum.Size = new System.Drawing.Size(156, 149);
			this.groupBoxSum.TabIndex = 4;
			this.groupBoxSum.TabStop = false;
			this.groupBoxSum.Text = "Adjustment Sum";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(6, 115);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(86, 18);
			this.label9.TabIndex = 154;
			this.label9.Text = "Amt End:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 93);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(86, 18);
			this.label12.TabIndex = 153;
			this.label12.Text = "Current Split:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(6, 71);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(86, 18);
			this.label6.TabIndex = 30;
			this.label6.Text = "Amt Avail:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 49);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 18);
			this.label3.TabIndex = 29;
			this.label3.Text = "Already Used:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(6, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 18);
			this.label2.TabIndex = 28;
			this.label2.Text = "Amt Original:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelAmtEnd
			// 
			this.labelAmtEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAmtEnd.Location = new System.Drawing.Point(98, 118);
			this.labelAmtEnd.Name = "labelAmtEnd";
			this.labelAmtEnd.Size = new System.Drawing.Size(52, 13);
			this.labelAmtEnd.TabIndex = 5;
			this.labelAmtEnd.Text = "0.00";
			// 
			// labelCurSplitAmt
			// 
			this.labelCurSplitAmt.Location = new System.Drawing.Point(98, 96);
			this.labelCurSplitAmt.Name = "labelCurSplitAmt";
			this.labelCurSplitAmt.Size = new System.Drawing.Size(52, 13);
			this.labelCurSplitAmt.TabIndex = 4;
			this.labelCurSplitAmt.Text = "0.00";
			// 
			// labelAmtAvail
			// 
			this.labelAmtAvail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAmtAvail.Location = new System.Drawing.Point(98, 74);
			this.labelAmtAvail.Name = "labelAmtAvail";
			this.labelAmtAvail.Size = new System.Drawing.Size(52, 13);
			this.labelAmtAvail.TabIndex = 3;
			this.labelAmtAvail.Text = "0.00";
			// 
			// labelAmtUsed
			// 
			this.labelAmtUsed.Location = new System.Drawing.Point(98, 52);
			this.labelAmtUsed.Name = "labelAmtUsed";
			this.labelAmtUsed.Size = new System.Drawing.Size(52, 13);
			this.labelAmtUsed.TabIndex = 2;
			this.labelAmtUsed.Text = "0.00";
			// 
			// labelAmtOriginal
			// 
			this.labelAmtOriginal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAmtOriginal.Location = new System.Drawing.Point(98, 30);
			this.labelAmtOriginal.Name = "labelAmtOriginal";
			this.labelAmtOriginal.Size = new System.Drawing.Size(52, 13);
			this.labelAmtOriginal.TabIndex = 1;
			this.labelAmtOriginal.Text = "0.00";
			// 
			// gridAdjusts
			// 
			this.gridAdjusts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAdjusts.EditableEnterMovesDown = false;
			this.gridAdjusts.Location = new System.Drawing.Point(16, 51);
			this.gridAdjusts.Name = "gridAdjusts";
			this.gridAdjusts.Size = new System.Drawing.Size(322, 336);
			this.gridAdjusts.TabIndex = 5;
			this.gridAdjusts.Title = "Unattached Adjustments";
			this.gridAdjusts.TranslationName = "gridAdjusts";
			this.gridAdjusts.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAdjusts_CellDoubleClick);
			this.gridAdjusts.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAdjusts_CellClick);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(485, 35);
			this.label1.TabIndex = 6;
			this.label1.Text = "Adjustments listed below are unattached to procedures and aren\'t fully allocated " +
    "via paysplits.  Select which Adjustment to use.";
			// 
			// FormAdjustSelect
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(509, 429);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridAdjusts);
			this.Controls.Add(this.groupBoxSum);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(525, 467);
			this.Name = "FormAdjustSelect";
			this.Text = "Adjustment Select";
			this.Load += new System.EventHandler(this.FormAdjustSelect_Load);
			this.groupBoxSum.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.GroupBox groupBoxSum;
		private UI.ODGrid gridAdjusts;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelAmtAvail;
		private System.Windows.Forms.Label labelAmtUsed;
		private System.Windows.Forms.Label labelAmtOriginal;
		private System.Windows.Forms.Label labelAmtEnd;
		private System.Windows.Forms.Label labelCurSplitAmt;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label9;
	}
}