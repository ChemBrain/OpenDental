using CodeBase;
using DataConnectionBase;
using OpenDental.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormQueryMonitor:ODForm {
		///<summary>Queries will be coming in at an alarming rate and this queue will get filled via DbMonitor events that are fired.
		///There will be a timer running on the UI thread that will be pulling queries out of this queue and storing them into a dictionary.</summary>
		private ConcurrentQueue<DbQueryObj> _queueQueries=new ConcurrentQueue<DbQueryObj>();
		///<summary>Stores all query objects that this instance of the query monitor has captured.</summary>
		private Dictionary<Guid,DbQueryObj> _dictQueries=new Dictionary<Guid,DbQueryObj>();
		///<summary>Shallow copy of the currently selected query object.  Do not trust the grid because it gets truncated as the feed grows.</summary>
		private DbQueryObj _dbQueryObjCur=null;

		///<summary>Returns true if this tool is currently monitoring queries.  Otherwise; false.</summary>
		private bool _isMonitoring {
			get {
				return (butStart.Text=="Stop");
			}
		}

		public FormQueryMonitor() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormQueryMonitor_Load(object sender,EventArgs e) {
			FillGridColumns();
		}

		private void TimerProcessQueue_Tick(object sender,EventArgs e) {
			if(_queueQueries.Count==0) {
				return;
			}
			List<DbQueryObj> listQueries=new List<DbQueryObj>();
			while(_queueQueries.Count!=0) {
				if(!_queueQueries.TryDequeue(out DbQueryObj query)) {
					break;
				}
				listQueries.Add(query);
			}
			AddQueriesToGrid(listQueries.ToArray());
		}

		private void FillGridColumns() {
			gridFeed.BeginUpdate();
			gridFeed.ListGridColumns.Clear();
			gridFeed.ListGridColumns.Add(new GridColumn(Lan.g(gridFeed.TranslationName,"Command"),-1));
			gridFeed.ListGridColumns.Add(new GridColumn(
				Lan.g(gridFeed.TranslationName,"DateTimeStart"),125,HorizontalAlignment.Center,GridSortingStrategy.DateParse)
			);
			gridFeed.ListGridColumns.Add(new GridColumn(Lan.g(gridFeed.TranslationName,"Elapsed"),100,HorizontalAlignment.Center));
			gridFeed.EndUpdate();
		}

		private void GridFeed_CellClick(object sender,ODGridClickEventArgs e) {
			textDateTimeStart.Clear();
			textDateTimeStop.Clear();
			textElapsed.Clear();
			textCommand.Clear();
			DbQueryObj dbQueryObj=gridFeed.ListGridRows[e.Row].Tag as DbQueryObj;
			if(dbQueryObj==null) {
				return;
			}
			ODException.SwallowAnyException(() => {
				_dbQueryObjCur=dbQueryObj;
				textDateTimeStart.Text=dbQueryObj.DateTimeStart.ToString();
				textDateTimeStop.Text=dbQueryObj.DateTimeStart.ToString();
				textElapsed.Text=dbQueryObj.Elapsed.ToString("G");
				textCommand.Text=dbQueryObj.Command;
			});
		}

		private void AddQueriesToGrid(params DbQueryObj[] arrayQueries) {
			if(arrayQueries.IsNullOrEmpty()) {
				return;
			}
			foreach(DbQueryObj query in arrayQueries) {
				_dictQueries[query.GUID]=query;//Refresh the object within the dictionary so that any new information(e.g.Elapsed) gets updated.
			}
			//Arbitrarily limit the number of rows showing in the grid.
			//The top of the grid is the oldest query so take the last X items.
			//Use TakeLast(X) instead of a loop because a bunch of queries could have been queued (e.g. pasting schedules can queue thousands).
			List<DbQueryObj> listDisplayQueries=_dictQueries.Values.TakeLast(500).ToList();
			gridFeed.BeginUpdate();
			gridFeed.ListGridRows.Clear();
			foreach(DbQueryObj query in listDisplayQueries) {
				GridRow row=new GridRow(query.Command,
					query.DateTimeInit.ToString(),
					(query.Elapsed==TimeSpan.MinValue) ? "" : query.Elapsed.ToString("G"))
				{
					Tag=query
				};
				gridFeed.ListGridRows.Add(row);
			}
			gridFeed.EndUpdate();
			gridFeed.ScrollToIndex(gridFeed.ListGridRows.Count-1);
		}

		private void ButStart_Click(object sender,EventArgs e) {
			if(_isMonitoring) {
				timerProcessQueue.Stop();
				QueryMonitorEvent.Fired-=DbMonitorEvent_Fired;
				butStart.Text="Start";
			}
			else {
				timerProcessQueue.Start();
				QueryMonitorEvent.Fired+=DbMonitorEvent_Fired;
				butStart.Text="Stop";
			}
		}

		private void DbMonitorEvent_Fired(ODEventArgs e) {
			if(e.EventType!=ODEventType.QueryMonitor || !(e.Tag is DbQueryObj)) {
				return;
			}
			_queueQueries.Enqueue(e.Tag as DbQueryObj);
		}

		private void ButLog_Click(object sender,EventArgs e) {
			if(_isMonitoring) {
				MsgBox.Show(this,"Stop monitoring queries before creating a log.");
				return;
			}
			if(_dictQueries.Count==0) {
				MsgBox.Show(this,"No queries in the Query Feed to log.");
				return;
			}
			if(ODBuild.IsWeb()) {
				MsgBox.Show(this,"Logging not supported for the Web version at this time.");
				return;
			}
			if(!MsgBox.Show(MsgBoxButtons.YesNo,Lan.g(this,"Log all queries to a file?  Total query count")+$": {_dictQueries.Count.ToString("N0")}")) {
				return;
			}
			string logFolderPath="QueryMonitorLogs";
			string logFileName="";
			try {
				//Create the query monitor log folder in the AtoZ image path.
				if(!OpenDentBusiness.FileIO.FileAtoZ.DirectoryExistsRelative(logFolderPath)) {
					OpenDentBusiness.FileIO.FileAtoZ.CreateDirectoryRelative(logFolderPath);
				}
				//Get a unique file name within the log folder.
				logFileName=$"QueryMonitorLog_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
				while(OpenDentBusiness.FileIO.FileAtoZ.ExistsRelative(logFolderPath,logFileName)) {
					logFileName=$"QueryMonitorLog_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
					Thread.Sleep(100);
				}
				//Dump the entire query history into the log file.
				OpenDentBusiness.FileIO.FileAtoZ.WriteAllTextRelative(logFolderPath,logFileName,
					$"Query Monitor Log - {DateTime.Now.ToString()}\r\n{string.Join("\r\n",_dictQueries.Values.Select(x => x.ToString()))}");
			}
			catch(ODException ode) {
				MsgBox.Show(ode.Message);
				return;
			}
			catch(Exception ex) {
				MsgBox.Show(Lan.g(this,"Error creating log file")+$":\r\n{ex.Message}");
				return;
			}
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Log file created.  Would you like to open the file?")) {
				try {
					FileAtoZ.StartProcessRelative(logFolderPath,logFileName);
				}
				catch(Exception ex) {
					MsgBox.Show(Lan.g(this,"Could not open log file")+$":\r\n{ex.Message}");
				}
			}
		}

		private void ButCopy_Click(object sender,EventArgs e) {
			if(_dbQueryObjCur==null) {
				MsgBox.Show(this,"Select a row from the Query Feed.");
				return;
			}
			try {
				Clipboard.SetText(_dbQueryObjCur.ToString());
				MsgBox.Show(this,"Copied");
			}
			catch(Exception ex) {
				ex.DoNothing();
				MsgBox.Show(this,"Could not copy contents to clipboard.  Please try again.");
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void FormQueryMonitor_FormClosing(object sender,FormClosingEventArgs e) {
			ODException.SwallowAnyException(() => { QueryMonitorEvent.Fired-=DbMonitorEvent_Fired; });
		}
	}
}