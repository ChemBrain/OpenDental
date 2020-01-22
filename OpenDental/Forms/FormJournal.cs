using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormJournal:ODForm {
		private OpenDental.UI.ODToolBar ToolBarMain;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.ODGrid gridMainPrint;
		private IContainer components;
		private Account _acctCur;
		private ImageList imageListMain;
		private bool _headingPrinted;
		private int _pagesPrinted;
		private int _headingPrintH;
		private Label labelDateFrom;
		private ValidDate textDateFrom;
		private ValidDate textDateTo;
		private Label labelDateTo;
		private OpenDental.UI.Button butRefresh;
		private MonthCalendar calendarFrom;
		private OpenDental.UI.Button butDropFrom;
		private OpenDental.UI.Button butDropTo;
		private MonthCalendar calendarTo;
		private ValidDouble textAmt;
		private Label label3;
		private Label label4;
		private TextBox textFindText;
		private int _prevGridWidth;
		private List<JournalEntry> _listJEntries;
		private Dictionary<long,long> _dictTransUsers;

		//set this externally so that the ending balances will match what's showing in the Chart of Accounts.
		public DateTime InitialAsOfDate;

		///<summary></summary>
		public FormJournal(Account accountCur)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_acctCur=accountCur;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJournal));
			this.imageListMain = new System.Windows.Forms.ImageList(this.components);
			this.gridMain = new OpenDental.UI.ODGrid();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			this.labelDateFrom = new System.Windows.Forms.Label();
			this.textDateFrom = new OpenDental.ValidDate();
			this.textDateTo = new OpenDental.ValidDate();
			this.labelDateTo = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.calendarFrom = new System.Windows.Forms.MonthCalendar();
			this.butDropFrom = new OpenDental.UI.Button();
			this.butDropTo = new OpenDental.UI.Button();
			this.calendarTo = new System.Windows.Forms.MonthCalendar();
			this.textAmt = new OpenDental.ValidDouble();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textFindText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// imageListMain
			// 
			this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
			this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListMain.Images.SetKeyName(0, "Add.gif");
			this.imageListMain.Images.SetKeyName(1, "print.gif");
			this.imageListMain.Images.SetKeyName(2, "butExport.gif");
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(0, 56);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(1044, 615);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = null;
			this.gridMain.TranslationName = "TableJournal";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListMain;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(1044, 25);
			this.ToolBarMain.TabIndex = 0;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// labelDateFrom
			// 
			this.labelDateFrom.Location = new System.Drawing.Point(6, 34);
			this.labelDateFrom.Name = "labelDateFrom";
			this.labelDateFrom.Size = new System.Drawing.Size(71, 17);
			this.labelDateFrom.TabIndex = 0;
			this.labelDateFrom.Text = "From";
			this.labelDateFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(78, 32);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(81, 20);
			this.textDateFrom.TabIndex = 1;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(268, 32);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(81, 20);
			this.textDateTo.TabIndex = 5;
			// 
			// labelDateTo
			// 
			this.labelDateTo.Location = new System.Drawing.Point(195, 34);
			this.labelDateTo.Name = "labelDateTo";
			this.labelDateTo.Size = new System.Drawing.Size(72, 17);
			this.labelDateTo.TabIndex = 0;
			this.labelDateTo.Text = "To";
			this.labelDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butRefresh
			// 
			this.butRefresh.Location = new System.Drawing.Point(711, 31);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 23);
			this.butRefresh.TabIndex = 10;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// calendarFrom
			// 
			this.calendarFrom.Location = new System.Drawing.Point(5, 56);
			this.calendarFrom.MaxSelectionCount = 1;
			this.calendarFrom.Name = "calendarFrom";
			this.calendarFrom.TabIndex = 4;
			this.calendarFrom.Visible = false;
			this.calendarFrom.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarFrom_DateSelected);
			// 
			// butDropFrom
			// 
			this.butDropFrom.Location = new System.Drawing.Point(161, 31);
			this.butDropFrom.Name = "butDropFrom";
			this.butDropFrom.Size = new System.Drawing.Size(22, 22);
			this.butDropFrom.TabIndex = 3;
			this.butDropFrom.Text = "V";
			this.butDropFrom.UseVisualStyleBackColor = true;
			this.butDropFrom.Click += new System.EventHandler(this.butDropFrom_Click);
			// 
			// butDropTo
			// 
			this.butDropTo.Location = new System.Drawing.Point(351, 31);
			this.butDropTo.Name = "butDropTo";
			this.butDropTo.Size = new System.Drawing.Size(22, 22);
			this.butDropTo.TabIndex = 6;
			this.butDropTo.Text = "V";
			this.butDropTo.UseVisualStyleBackColor = true;
			this.butDropTo.Click += new System.EventHandler(this.butDropTo_Click);
			// 
			// calendarTo
			// 
			this.calendarTo.Location = new System.Drawing.Point(231, 56);
			this.calendarTo.MaxSelectionCount = 1;
			this.calendarTo.Name = "calendarTo";
			this.calendarTo.TabIndex = 7;
			this.calendarTo.Visible = false;
			this.calendarTo.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarTo_DateSelected);
			// 
			// textAmt
			// 
			this.textAmt.ForeColor = System.Drawing.SystemColors.WindowText;
			this.textAmt.Location = new System.Drawing.Point(450, 32);
			this.textAmt.MaxVal = 100000000D;
			this.textAmt.MinVal = -100000000D;
			this.textAmt.Name = "textAmt";
			this.textAmt.Size = new System.Drawing.Size(81, 20);
			this.textAmt.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(386, 34);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 17);
			this.label3.TabIndex = 0;
			this.label3.Text = "Find Amt";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(541, 34);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "Find Text";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textFindText
			// 
			this.textFindText.Location = new System.Drawing.Point(605, 32);
			this.textFindText.Name = "textFindText";
			this.textFindText.Size = new System.Drawing.Size(78, 20);
			this.textFindText.TabIndex = 9;
			// 
			// FormJournal
			// 
			this.ClientSize = new System.Drawing.Size(1044, 671);
			this.Controls.Add(this.calendarFrom);
			this.Controls.Add(this.calendarTo);
			this.Controls.Add(this.textFindText);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textAmt);
			this.Controls.Add(this.butDropTo);
			this.Controls.Add(this.butDropFrom);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.labelDateTo);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.labelDateFrom);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.ToolBarMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(890, 270);
			this.Name = "FormJournal";
			this.ShowInTaskbar = false;
			this.Text = "Transaction History";
			this.Load += new System.EventHandler(this.FormJournal_Load);
			this.ResizeEnd += new System.EventHandler(this.FormJournal_ResizeEnd);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormJournal_Load(object sender,EventArgs e) {
			DateTime firstofYear=new DateTime(InitialAsOfDate.Year,1,1);
			textDateTo.Text=InitialAsOfDate.ToShortDateString();
			if(_acctCur.AcctType==AccountType.Income || _acctCur.AcctType==AccountType.Expense){
				textDateFrom.Text=firstofYear.ToShortDateString();
			}
			LayoutToolBar();
			FillGrid();
			gridMain.ScrollToEnd();
			_prevGridWidth=gridMain.Width;
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Entry"),0,"","Add"));
			if(_acctCur.AcctType==AccountType.Asset){
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Reconcile"),-1,"","Reconcile"));
			}
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print"),1,"","Print"));
			//ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Edit"),-1,Lan.g(this,"Edit Selected Account"),"Edit"));
			//ODToolBarButton button=new ODToolBarButton("",-1,"","PageNum");
			//button.Style=ODToolBarButtonStyle.Label;
			//ToolBarMain.Buttons.Add(button);
			//ToolBarMain.Buttons.Add(new ODToolBarButton("",2,"Go Forward One Page","Fwd"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Export"),2,Lan.g(this,"Export the Account Grid"),"Export"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,"Close This Window","Close"));
		}

		private void ToolBarMain_ButtonClick(object sender,OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Add":
					Add_Click();
					break;
				case "Reconcile":
					Reconcile_Click();
					break;
				case "Print":
					//The reason we are using a delegate and BeginInvoke() is because of a Microsoft bug that causes the Print Dialog window to not be in focus			
					//when it comes from a toolbar click.
					//https://social.msdn.microsoft.com/Forums/windows/en-US/681a50b4-4ae3-407a-a747-87fb3eb427fd/first-mouse-click-after-showdialog-hits-the-parent-form?forum=winforms
					ToolBarClick toolClick=Print_Click;
					this.BeginInvoke(toolClick);
					break;
				case "Export":
					Export_Click();
					break;
				case "Close":
					this.Close();
					break;
			}
		}

		private delegate void ToolBarClick();

		private void FillGrid(bool isPrinting=false,bool isResizing=false) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!="" || textDateTo.errorProvider1.GetError(textDateTo)!="") {
				return;
			}
			ODGrid gridToFill=isPrinting?gridMainPrint:gridMain;
			long firstVisibleTransNum=0;
			if(!isPrinting && gridToFill.VisibleRows.Count>0) {//don't scroll into view if printing
				firstVisibleTransNum=(long)gridToFill.VisibleRows[0].Tag;
			}
			long selectedTransNum=0;
			if(!isPrinting && gridToFill.GetSelectedIndex()>-1) {//no need to reselect an index if printing
				selectedTransNum=gridToFill.SelectedTag<long>();
			}
			//Resize grid to fit, important for later resizing
			gridToFill.BeginUpdate();
			gridToFill.Title=_acctCur.Description+" ("+Lan.g("enumAccountType",_acctCur.AcctType.ToString())+")";
			FillColumns(isPrinting,isResizing);
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=string.IsNullOrEmpty(textDateTo.Text)?DateTime.MaxValue:PIn.Date(textDateTo.Text);
			double filterAmt=string.IsNullOrEmpty(textAmt.errorProvider1.GetError(textAmt))?PIn.Double(textAmt.Text):0;
			if(!isResizing || _listJEntries==null || _dictTransUsers==null) {
				_listJEntries=JournalEntries.GetForAccount(_acctCur.AccountNum);
				_dictTransUsers=Transactions.GetManyTrans(_listJEntries.Select(x => x.TransactionNum).ToList())
					.ToDictionary(x => x.TransactionNum,x => x.UserNum);
			}
			gridToFill.ListGridRows.Clear();
			GridRow row;
			decimal bal=0;
			int firstVisibleRowIndex=-1;
			int selectedIndex=-1;
			foreach(JournalEntry jeCur in _listJEntries) {
				if(jeCur.DateDisplayed>dateTo) {
					break;
				}
				if(new[] { AccountType.Income,AccountType.Expense }.Contains(_acctCur.AcctType) && jeCur.DateDisplayed<dateFrom) {
					continue;//for income and expense accounts, previous balances are not included. Only the current timespan.
				}
				//DebitIsPos=true for checking acct, bal+=DebitAmt-CreditAmt
				bal+=(Accounts.DebitIsPos(_acctCur.AcctType)?1:-1)*((decimal)jeCur.DebitAmt-(decimal)jeCur.CreditAmt);
				if(new[] { AccountType.Asset,AccountType.Liability,AccountType.Equity }.Contains(_acctCur.AcctType) && jeCur.DateDisplayed<dateFrom) {
					continue;//for asset, liability, and equity accounts, older entries do affect the current balance.
				}
				if(filterAmt!=0 && filterAmt!=jeCur.CreditAmt && filterAmt!=jeCur.DebitAmt){
					continue;
				}
				if(textFindText.Text!="" && new[] { jeCur.Memo,jeCur.CheckNumber,jeCur.Splits }.All(x => !x.ToUpper().Contains(textFindText.Text.ToUpper()))) {
					continue;
				}
				row=new GridRow();
				row.Cells.Add(jeCur.CheckNumber);
				row.Cells.Add(jeCur.DateDisplayed.ToShortDateString());
				row.Cells.Add(jeCur.Memo);
				row.Cells.Add(jeCur.Splits);
				row.Cells.Add(jeCur.DebitAmt==0?"":jeCur.DebitAmt.ToString("n"));
				row.Cells.Add(jeCur.CreditAmt==0?"":jeCur.CreditAmt.ToString("n"));
				row.Cells.Add(bal.ToString("n"));
				long userNum;
				row.Cells.Add(Userods.GetName(_dictTransUsers.TryGetValue(jeCur.TransactionNum,out userNum)?userNum:0));
				row.Cells.Add(Userods.GetName(jeCur.SecUserNumEdit));
				row.Cells.Add(jeCur.ReconcileNum==0?"":"X");
				row.Tag=jeCur.TransactionNum;
				gridToFill.ListGridRows.Add(row);
				if(firstVisibleTransNum>0 && jeCur.TransactionNum==firstVisibleTransNum) {
					firstVisibleRowIndex=gridToFill.ListGridRows.Count-1;
				}
				if(selectedTransNum>0 && jeCur.TransactionNum==selectedTransNum) {
					selectedIndex=gridToFill.ListGridRows.Count-1;
				}
			}
			gridToFill.EndUpdate();
			if(selectedIndex>-1) {
				gridToFill.SetSelected(selectedIndex,true);
			}
			if(firstVisibleRowIndex>-1) {
				gridToFill.ScrollToIndex(firstVisibleRowIndex);
			}
			else {
				gridToFill.ScrollToEnd();
			}
		}

		private void FillColumns(bool isPrinting,bool isResizing) {
			ODGrid gridToFill=isPrinting?gridMainPrint:gridMain;
			List<string> listColHeadings=new List<string>(new[] { "Chk #","Date","Memo","Splits",
				"Debit"+(Accounts.DebitIsPos(_acctCur.AcctType)?"(+)":"(-)"),
				"Credit"+(Accounts.DebitIsPos(_acctCur.AcctType)?"(-)":"(+)"),
				"Balance","Created By","Last Edited By","Clear" });
			Dictionary<string,Tuple<int,HorizontalAlignment>> dictColWidths=new Dictionary<string,Tuple<int,HorizontalAlignment>>();
			dictColWidths["Chk #"]=Tuple.Create(60,HorizontalAlignment.Center);
			dictColWidths["Date"]=Tuple.Create(75,HorizontalAlignment.Left);
			dictColWidths["Memo"]=Tuple.Create((isPrinting?200:220),HorizontalAlignment.Left);
			dictColWidths["Splits"]=Tuple.Create((isPrinting?200:220),HorizontalAlignment.Left);
			dictColWidths["Created By"]=Tuple.Create(100,HorizontalAlignment.Left);
			dictColWidths["Last Edited By"]=Tuple.Create(100,HorizontalAlignment.Left);
			dictColWidths["Clear"]=Tuple.Create(40,HorizontalAlignment.Center);
			//grid width minus scroll bar width if not printing minus width of all cols except Debit, Credit, and Balance and divide by 3
			//distribute the remaining grid width between the three cols: Debit, Credit, and Balance
			int colW=(gridToFill.Width-(isPrinting?0:19)-dictColWidths.Values.Sum(x => x.Item1))/3;
			dictColWidths["Debit"+(Accounts.DebitIsPos(_acctCur.AcctType)?"(+)":"(-)")]=Tuple.Create(colW,HorizontalAlignment.Right);
			dictColWidths["Credit"+(Accounts.DebitIsPos(_acctCur.AcctType)?"(-)":"(+)")]=Tuple.Create(colW,HorizontalAlignment.Right);
			dictColWidths["Balance"]=Tuple.Create(colW,HorizontalAlignment.Right);
			if((isPrinting || isResizing) && gridToFill.ListGridColumns!=null && gridToFill.ListGridColumns.Count>0) {//printing/resizing and cols already filled, adjust widths
				Tuple<int,HorizontalAlignment> colCurTuple;
				gridToFill.ListGridColumns.ForEach(x => x.ColWidth=(dictColWidths.TryGetValue(x.Heading,out colCurTuple)?colCurTuple.Item1:x.ColWidth));
			}
			else {//if not printing/resizing or cols have not been filled, fill cols
				gridToFill.ListGridColumns.Clear();
				listColHeadings.ForEach(x => gridToFill.ListGridColumns.Add(new GridColumn(Lan.g("TableJournal",x),dictColWidths[x].Item1,dictColWidths[x].Item2)));
			}
		}

		private void Add_Click(){
			Transaction trans=new Transaction();
			trans.UserNum=Security.CurUser.UserNum;
			Transactions.Insert(trans);//we now have a TransactionNum, and datetimeEntry has been set
			FormTransactionEdit FormT=new FormTransactionEdit(trans.TransactionNum,_acctCur.AccountNum);
			FormT.IsNew=true;
			FormT.ShowDialog();
			if(FormT.DialogResult==DialogResult.Cancel){
				//no need to try-catch, since no journal entries were saved.
				Transactions.Delete(trans);
			}
			FillGrid();
		}

		private void Reconcile_Click() {
			int selectedRow=gridMain.GetSelectedIndex();
			int scrollValue=gridMain.ScrollValue;
			FormReconciles FormR=new FormReconciles(_acctCur.AccountNum);
			FormR.ShowDialog();
			FillGrid();
			gridMain.SetSelected(selectedRow,true);
			gridMain.ScrollValue=scrollValue;
		}

		private void Print_Click(){
			_pagesPrinted=0;
			_headingPrinted=false;
			gridMainPrint=new ODGrid() { Width=1050,TranslationName="",HideScrollBars=true };
			FillGrid(isPrinting:true);//not nessecary to explicity name parameter but makes code easier to read.
			PrintoutOrientation orient=PrintoutOrientation.Default;
			if(gridMainPrint.WidthAllColumns>800) {
				orient=PrintoutOrientation.Landscape;
			}
			PrinterL.TryPrintOrDebugRpPreview(pd2_PrintPage,
				Lan.g(this,"Accounting transaction history for")+" "+_acctCur.Description+" "+Lan.g(this,"printed"),
				printoutOrientation:orient
			);
		}

		private void pd2_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//Rectangle bounds=new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			using(Graphics g = e.Graphics)
			using(Font headingFont=new Font("Arial",13,FontStyle.Bold))
			using(Font subHeadingFont=new Font("Arial",10,FontStyle.Bold)) {
				string text;
				int yPos=bounds.Top;
				int center=bounds.X+bounds.Width/2;
				#region printHeading
				if(!_headingPrinted) {
					text=_acctCur.Description+" ("+Lan.g("enumAccountType",_acctCur.AcctType.ToString())+")";
					g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
					yPos+=(int)g.MeasureString(text,headingFont).Height;
					text=DateTime.Today.ToShortDateString();
					g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					yPos+=20;
					_headingPrinted=true;
					_headingPrintH=yPos;
				}
				#endregion
				yPos=gridMainPrint.PrintPage(g,_pagesPrinted,bounds,_headingPrintH);
				_pagesPrinted++;
				if(yPos==-1) {
					e.HasMorePages=true;
				}
				else {
					e.HasMorePages=false;
				}
			}
		}

		private void Export_Click() {
			//FillGrid();//this is in case the date range has been changed but the grid has not been refreshed to reflect the new date range, so the date range on the report will match the data in the grid.
			List<Tuple<string,string>> listOtherDetails=new List<Tuple<string,string>>() {
				Tuple.Create(labelDateFrom.Text,PIn.Date(textDateFrom.Text).ToShortDateString()),
				Tuple.Create(labelDateTo.Text,PIn.Date(textDateTo.Text).ToShortDateString())
			};
			string msg=gridMain.Export(listOtherDetails:listOtherDetails);
			if(!string.IsNullOrEmpty(msg)) {
				MsgBox.Show(this,msg);
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			int selectedRow=e.Row;
			int scrollValue=gridMain.ScrollValue;
			FormTransactionEdit FormT=new FormTransactionEdit((long)gridMain.ListGridRows[e.Row].Tag,_acctCur.AccountNum);
			FormT.ShowDialog();
			if(FormT.DialogResult==DialogResult.Cancel) {
				return;
			}
			FillGrid();
			gridMain.SetSelected(selectedRow,true);
			gridMain.ScrollValue=scrollValue;
		}

		private void butDropFrom_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void butDropTo_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void ToggleCalendars(){
			if(calendarFrom.Visible){
				//hide the calendars
				calendarFrom.Visible=false;
				calendarTo.Visible=false;
				FillGrid();
			}
			else{
				//set the date on the calendars to match what's showing in the boxes
				if(textDateFrom.errorProvider1.GetError(textDateFrom)==""
					&& textDateTo.errorProvider1.GetError(textDateTo)=="")
				{//if no date errors
					if(textDateFrom.Text==""){
						calendarFrom.SetDate(DateTime.Today);
					}
					else{
						calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
					}
					if(textDateTo.Text=="") {
						calendarTo.SetDate(DateTime.Today);
					}
					else {
						calendarTo.SetDate(PIn.Date(textDateTo.Text));
					}
				}
				//show the calendars
				calendarFrom.Visible=true;
				calendarTo.Visible=true;
			}
		}

		private void calendarFrom_DateSelected(object sender,DateRangeEventArgs e) {
			textDateFrom.Text=calendarFrom.SelectionStart.ToShortDateString();
		}

		private void calendarTo_DateSelected(object sender,DateRangeEventArgs e) {
			textDateTo.Text=calendarTo.SelectionStart.ToShortDateString();
		}

		///<summary>Fires when resizing is complete.  Window doesn't have maximize or minimize, so only need to handle manual resizing.
		///Also fires when the form is moved so we keep track of the grid width to prevent a move from causing a FillGrid.</summary>
		private void FormJournal_ResizeEnd(object sender,EventArgs e) {
			if(gridMain.Width==_prevGridWidth) {
				return;
			}
			FillGrid(isResizing:true);
			_prevGridWidth=gridMain.Width;
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				|| textAmt.errorProvider1.GetError(textAmt)!=""
				)
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			calendarFrom.Visible=false;
			calendarTo.Visible=false;
			_listJEntries=null;//set to null to force a refresh
			_dictTransUsers=null;//set to null to force a refresh
			FillGrid();
		}

	}
}





















