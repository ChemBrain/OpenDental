using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using System.Text;
using CodeBase;
using System.IO;
using OpenDental.Thinfinity;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormTimeCardBenefitRp:ODForm {
		private UI.Button butPrint;
		private ODGrid gridMain;
		private UI.Button butClose;
		private CheckBox checkShowAll;
		private CheckBox checkIgnore;
		private DateTime _dtNow;
		private DateTime _monthT0;
		private DateTime _monthT1;
		private DateTime _monthT2;
		private UI.Button butExportGrid;
		private Color lightRed;

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.butPrint = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butClose = new OpenDental.UI.Button();
			this.checkShowAll = new System.Windows.Forms.CheckBox();
			this.checkIgnore = new System.Windows.Forms.CheckBox();
			this.butExportGrid = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butPrint
			// 
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(519, 660);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(86, 24);
			this.butPrint.TabIndex = 22;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(726, 642);
			this.gridMain.TabIndex = 21;
			this.gridMain.Title = "";
			this.gridMain.TranslationName = "TableTimeCard";
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(663, 660);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 20;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// checkShowAll
			// 
			this.checkShowAll.AutoSize = true;
			this.checkShowAll.Location = new System.Drawing.Point(12, 665);
			this.checkShowAll.Name = "checkShowAll";
			this.checkShowAll.Size = new System.Drawing.Size(67, 17);
			this.checkShowAll.TabIndex = 23;
			this.checkShowAll.Text = "Show All";
			this.checkShowAll.UseVisualStyleBackColor = true;
			this.checkShowAll.CheckedChanged += new System.EventHandler(this.checkShowAll_CheckedChanged);
			// 
			// checkIgnore
			// 
			this.checkIgnore.AutoSize = true;
			this.checkIgnore.Checked = true;
			this.checkIgnore.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkIgnore.Location = new System.Drawing.Point(85, 665);
			this.checkIgnore.Name = "checkIgnore";
			this.checkIgnore.Size = new System.Drawing.Size(124, 17);
			this.checkIgnore.TabIndex = 24;
			this.checkIgnore.Text = "Ignore current month";
			this.checkIgnore.UseVisualStyleBackColor = true;
			this.checkIgnore.CheckedChanged += new System.EventHandler(this.checkIgnore_CheckedChanged);
			// 
			// butExportGrid
			// 
			this.butExportGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butExportGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExportGrid.Location = new System.Drawing.Point(431, 660);
			this.butExportGrid.Name = "butExportGrid";
			this.butExportGrid.Size = new System.Drawing.Size(82, 24);
			this.butExportGrid.TabIndex = 128;
			this.butExportGrid.Text = "Export Grid";
			this.butExportGrid.Click += new System.EventHandler(this.butExportGrid_Click);
			// 
			// FormTimeCardBenefitRp
			// 
			this.ClientSize = new System.Drawing.Size(750, 696);
			this.Controls.Add(this.butExportGrid);
			this.Controls.Add(this.checkIgnore);
			this.Controls.Add(this.checkShowAll);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Name = "FormTimeCardBenefitRp";
			this.Text = "Benefit Eligibility Letters Report";
			this.Load += new System.EventHandler(this.FormTimeCardBenefitRp_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		public FormTimeCardBenefitRp() {
			InitializeComponent();
		}

		private void FormTimeCardBenefitRp_Load(object sender,EventArgs e) {
			_dtNow = MiscData.GetNowDateTime();
			this.Text+=" - "+_dtNow.ToLongDateString();
			_monthT0 = new DateTime(_dtNow.Year,_dtNow.Month,1);//.AddMonths(-0);
			_monthT1 = _monthT0.AddMonths(-1);
			_monthT2 = _monthT0.AddMonths(-2);
			lightRed=Color.FromArgb(254,235,233);//light red
			FillGridMain();
		}

		private void FillGridMain() {
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g(this,"LName"),100));
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g(this,"FName"),100));
			gridMain.ListGridColumns.Add(new GridColumn(_monthT2.ToString("MMMM yyyy"),100,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridMain.ListGridColumns.Add(new GridColumn(_monthT1.ToString("MMMM yyyy"),100,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			if(!checkIgnore.Checked) {
				gridMain.ListGridColumns.Add(new GridColumn(_monthT0.ToString("MMMM yyyy"),100,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			}
			gridMain.ListGridColumns.Add(new GridColumn(Lan.g(this,"Letter"),100));
			gridMain.ListGridRows.Clear();
			List<Employee> listEmpsAll = Employees.GetDeepCopy(true).OrderBy(x => x.LName).ThenBy(x => x.FName).ToList();
			List<ClockEvent> listClockEventsAll = ClockEvents.GetAllForPeriod(_monthT2,_monthT2.AddMonths(3));//get all three months of clock events
			listClockEventsAll.RemoveAll(x => x.ClockStatus==TimeClockStatus.Break);//remove breaks, they have already been acounted for on the clock events.
			listClockEventsAll.RemoveAll(x => x.TimeDisplayed2<=x.TimeDisplayed1);//Remove all mal-formed entries with stop time before start time. (also if user has not clocked out.)
			listClockEventsAll.RemoveAll(x => x.TimeDisplayed1.Date!=x.TimeDisplayed2.Date);//No one works over midnight at ODHQ. If they do, they know to split clock events @ midnight
			List<TimeAdjust> listTimeAdjustAll = TimeAdjusts.GetAllForPeriod(_monthT2,_monthT2.AddMonths(3));
			foreach(Employee empCur in listEmpsAll) {
				//Construct each row, then filter out if neccesary.
				GridRow row = new GridRow();
				//Name
				row.Cells.Add(empCur.LName);
				row.Cells.Add(empCur.FName);
				//Month T-2 (current month -2 months)
				TimeSpan ts2 = TimeSpan.FromTicks(listClockEventsAll
					.FindAll(x => x.EmployeeNum==empCur.EmployeeNum
						&& x.TimeDisplayed1.Year==_monthT2.Year
						&& x.TimeDisplayed1.Month==_monthT2.Month)
					.Select(x => (x.TimeDisplayed2-x.TimeDisplayed1)+(x.AdjustIsOverridden ? x.Adjust : x.AdjustAuto))
					.Sum(x=>x.Ticks));
				ts2.Add(TimeSpan.FromTicks(listTimeAdjustAll.FindAll(x => x.EmployeeNum==empCur.EmployeeNum
						&& x.TimeEntry.Year==_monthT2.Year
						&& x.TimeEntry.Month==_monthT2.Month)
						.Sum(x => x.RegHours.Ticks)));
				row.Cells.Add(new GridCell(string.Format("{0:0.00}",Math.Round(ts2.TotalHours,2,MidpointRounding.AwayFromZero))) { ColorBackG=(ts2.TotalHours<125 ? lightRed : Color.Empty) });
				//Month T-1
				TimeSpan ts1 = TimeSpan.FromTicks(listClockEventsAll
					.FindAll(x => x.EmployeeNum==empCur.EmployeeNum
						&& x.TimeDisplayed1.Year==_monthT1.Year
						&& x.TimeDisplayed1.Month==_monthT1.Month)
					.Select(x => (x.TimeDisplayed2-x.TimeDisplayed1)+(x.AdjustIsOverridden ? x.Adjust : x.AdjustAuto))
					.Sum(x => x.Ticks));
				ts1.Add(TimeSpan.FromTicks(listTimeAdjustAll.FindAll(x => x.EmployeeNum==empCur.EmployeeNum
						&& x.TimeEntry.Year==_monthT1.Year
						&& x.TimeEntry.Month==_monthT1.Month)
						.Select(x => x.RegHours)
						.Sum(x => x.Ticks)));
				row.Cells.Add(new GridCell(string.Format("{0:0.00}",Math.Round(ts1.TotalHours,2,MidpointRounding.AwayFromZero))) { ColorBackG=(ts1.TotalHours<125 ? lightRed : Color.Empty) });
				//Month T-0
				TimeSpan ts0 = TimeSpan.FromTicks(listClockEventsAll
					.FindAll(x => x.EmployeeNum==empCur.EmployeeNum
						&& x.TimeDisplayed1.Year==_monthT0.Year
						&& x.TimeDisplayed1.Month==_monthT0.Month)
					.Select(x => (x.TimeDisplayed2-x.TimeDisplayed1)+(x.AdjustIsOverridden ? x.Adjust : x.AdjustAuto))
					.Sum(x => x.Ticks));
				ts0.Add(TimeSpan.FromTicks(listTimeAdjustAll.FindAll(x => x.EmployeeNum==empCur.EmployeeNum
						&& x.TimeEntry.Year==_monthT0.Year
						&& x.TimeEntry.Month==_monthT0.Month)
						.Select(x => x.RegHours)
						.Sum(x => x.Ticks)));
				if(!checkIgnore.Checked) {
					row.Cells.Add(new GridCell(string.Format("{0:0.00}",Math.Round(ts0.TotalHours,2,MidpointRounding.AwayFromZero))) { ColorBackG=(ts0.TotalHours<125 ? lightRed : Color.Empty) });
				}
				//filter out rows that should not be displaying. Rows should not display only when the most recent month was less than 125 hours, regardless of status of previous months
				if(!checkShowAll.Checked) {
					//Show all is not checked, therefore we must filter out rows with more than 125 hours on the most recent pay period.
					if(!checkIgnore.Checked && ts0.TotalHours>125) {
						//Not ignoring "current month" so check T0 for >125 hours
						continue;
					}
					else if(checkIgnore.Checked && ts1.TotalHours>125) {
						//Ignore current month (because it is probably only partially complete), check the previous months for >125 hours. 
						continue;
					}
				}
				string letterNumber = "";
				if((checkIgnore.Checked?ts2:ts0).TotalHours<125 && ts1.TotalHours<125) {
					//the last two visible month were less than 125 hours. AKA "Send the Second letter"
					letterNumber="Second";
				}
				else if((checkIgnore.Checked?ts1:ts0).TotalHours<125) {
					//the last visible month was less than 125 hours. AKA "Send the First letter"
					letterNumber="First";
				}
				row.Cells.Add(letterNumber);
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void checkShowAll_CheckedChanged(object sender,EventArgs e) {
			FillGridMain();
		}

		private void checkIgnore_CheckedChanged(object sender,EventArgs e) {
			FillGridMain();
		}

		#region Print Grid
		private int pagesPrinted;
		private bool headingPrinted;

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			headingPrinted=false;
			PrinterL.TryPrintOrDebugRpPreview(pd_PrintPage,Lan.g(this,"Payroll benefits report printed."));
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds = e.MarginBounds;
			Graphics g = e.Graphics;
			string text;
			Font headingFont = new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont = new Font("Arial",10,FontStyle.Bold);
			int yPos = bounds.Top;
			int center = bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Payroll Benefits Report");
				StringFormat sf = StringFormat.GenericDefault;
				sf.Alignment=StringAlignment.Center;
				g.DrawString(text,headingFont,Brushes.Black,center,yPos,sf);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				g.DrawString(_dtNow.ToString(),subHeadingFont,Brushes.Black,center,yPos,sf);
				yPos+=20;
				headingPrinted=true;
			}
			#endregion
			yPos=gridMain.PrintPage(g,pagesPrinted,bounds,yPos);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}
		#endregion Print Grid

		private void butExportGrid_Click(object sender,EventArgs e) {
			StringBuilder sb=new StringBuilder();
			sb.AppendLine(string.Join(",",gridMain.ListGridColumns.OfType<GridColumn>().Select(x => x.Heading)));
			gridMain.ListGridRows.OfType<GridRow>().ToList()
				.ForEach(row => sb.AppendLine(string.Join(",",row.Cells.Select(cell => cell.Text.Replace(',','-').Replace('\t',',')))));
			if(ODBuild.IsWeb()) {
				string exportFilename="BenefitsRpt_"+_dtNow.ToString("yyyyMMdd")+"_"+DateTime.Now.ToString("hhmm")+".csv";
				string dataString=sb.ToString();
				ThinfinityUtils.ExportForDownload(exportFilename,dataString);
				return;
			}
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if(fbd.ShowDialog()!=DialogResult.OK || string.IsNullOrEmpty(fbd.SelectedPath)) {
				MsgBox.Show(this,"Invalid directory.");
				return;
			}
			string filename = ODFileUtils.CombinePaths(fbd.SelectedPath,"BenefitsRpt_"+_dtNow.ToString("yyyyMMdd")+"_"+DateTime.Now.ToString("hhmm")+".csv");
			System.IO.File.WriteAllText(filename,sb.ToString());
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to open the file now?")) {
				try {
					System.Diagnostics.Process.Start(filename);
				}
				catch(Exception ex) { ex.DoNothing(); }
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}





















