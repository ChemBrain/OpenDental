using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CodeBase;
using System.Drawing.Printing;

namespace OpenDental{
///<summary></summary>
	public class FormRpPrintPreview : ODForm {
		///<summary></summary>
		private OpenDental.UI.ODPrintPreviewControl _printPreviewControl2;
		private OpenDental.UI.Button butNext;
		private OpenDental.UI.Button butPrev;
		private System.ComponentModel.Container components = null;


		///<summary></summary>
		public FormRpPrintPreview() {
			InitializeComponent();
 			Lan.F(this);
		}
		///<summary></summary>
		public FormRpPrintPreview(PrintDocument printDoc) : this() {
			_printPreviewControl2.Document=printDoc;
		}

		///<summary></summary>
		public FormRpPrintPreview(ODprintout printout) : this() {
			if(printout.SettingsErrorCode!=PrintoutErrorCode.Success) {
				PrinterL.ShowError(printout);
				this.DialogResult=DialogResult.Cancel;
				return;
			}
			_printPreviewControl2.Document=printout.PrintDoc;
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

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpPrintPreview));
			this._printPreviewControl2 = new OpenDental.UI.ODPrintPreviewControl();
			this.butNext = new OpenDental.UI.Button();
			this.butPrev = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// printPreviewControl2
			// 
			this._printPreviewControl2.AutoZoom = false;
			this._printPreviewControl2.Location = new System.Drawing.Point(0, 0);
			this._printPreviewControl2.Name = "printPreviewControl2";
			this._printPreviewControl2.Size = new System.Drawing.Size(842, 538);
			this._printPreviewControl2.TabIndex = 7;
			this._printPreviewControl2.Zoom = 1D;
			// 
			// button1
			// 
			this.butNext.Location = new System.Drawing.Point(323, 709);
			this.butNext.Name = "button1";
			this.butNext.Size = new System.Drawing.Size(75, 23);
			this.butNext.TabIndex = 8;
			this.butNext.Text = "Next Page";
			this.butNext.Click += new System.EventHandler(this.butNext_Click);
			// 
			// button2
			// 
			this.butPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrev.Location = new System.Drawing.Point(12, 709);
			this.butPrev.Name = "button2";
			this.butPrev.Size = new System.Drawing.Size(75, 23);
			this.butPrev.TabIndex = 9;
			this.butPrev.Text = "Prev. Page";
			this.butPrev.Click += new System.EventHandler(this.butPrev_Click);
			// 
			// FormRpPrintPreview
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(842, 746);
			this.Controls.Add(this.butPrev);
			this.Controls.Add(this.butNext);
			this.Controls.Add(this._printPreviewControl2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRpPrintPreview";
			this.ShowInTaskbar = false;
			this.Text = "FormRpPrintPreview";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.FormRpPrintPreview_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpPrintPreview_Load(object sender, System.EventArgs e) {
			butNext.Location=new Point(this.ClientRectangle.Width-100,this.ClientRectangle.Height-30);
			butPrev.Location=new Point(this.ClientRectangle.Width-butPrev.Width-110,this.ClientRectangle.Height-30);
			_printPreviewControl2.Height=this.ClientRectangle.Height-40;
			_printPreviewControl2.Width=this.ClientRectangle.Width;
			_printPreviewControl2.Zoom=(double)_printPreviewControl2.ClientSize.Height
				/1100;
		}

		private void butNext_Click(object sender,System.EventArgs e) {
			_printPreviewControl2.StartPage++;
		}

		private void butPrev_Click(object sender,EventArgs e) {
			if(_printPreviewControl2.StartPage>0) {
				_printPreviewControl2.StartPage--;
			}
		}
	}
}
