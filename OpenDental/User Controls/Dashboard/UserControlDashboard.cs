using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class UserControlDashboard:UserControl {
		private ODThread _threadRefresh;
		private bool _isLoggingOff=false;
		private Action _actionDashboardContentsChanged;
		private Action _actionDashboardClosing;
		private bool _isInitialized;
		private const string REFRESH_THREAD_NAME="DashboardRefresh_Thread";
		private const string SET_THREAD_NAME="DashboardSet_Thread";

		public List<UserControlDashboardWidget> ListOpenWidgets {
			get {
				return Controls.OfType<UserControlDashboardWidget>().ToList();
			}
		}

		///<summary>The width of the widest open UserControlDashboardWidget.</summary>
		public int WidgetWidth {
			get {
				if(ListOpenWidgets.IsNullOrEmpty()) {
					return 0;
				}
				return ListOpenWidgets.Max(x => x.Width);
			}
		}

		public bool IsInitialized {
			get{
				return _isInitialized;
			}
			private set {
				_isInitialized=value;
				//Unsubscribe first to avoid duplicate subscriptions, or subscription "leaks".
				PatientEvent.Fired-=PatientEvent_Fired;
				PatientChangedEvent.Fired-=PatientChangedEvent_Fired;
				PatientDashboardDataEvent.Fired-=PatientDashboardDataEvent_Fired;
				if(_isInitialized){
					PatientEvent.Fired+=PatientEvent_Fired;
					PatientChangedEvent.Fired+=PatientChangedEvent_Fired;
					PatientDashboardDataEvent.Fired+=PatientDashboardDataEvent_Fired;
				}
			}
		}

		public bool IsSettingData { get; private set; }

		public UserControlDashboard() {
			InitializeComponent();
			ContextMenuStrip=contextMenu;
		}

		/// <summary>Initializes a Dashboard.</summary>
		/// <param name="sheetDefDashboard">SheetDef for the Patient Dashboard to open.</param>
		/// <param name="actionOnDashboardContentsChanged">Action is fired when the contents of the Dashboard change.</param>
		/// <param name="actionOnDashboardClosing">Action is fired when the Dashboard is closing.</param>
		/// in the Appointment Edit window. </param>
		public void Initialize(SheetDef sheetDefDashboard,Action actionOnDashboardContentsChanged,Action actionOnDashboardClosing) {
			if(sheetDefDashboard==null) {
				return;
			}
			AddWidget(sheetDefDashboard);
			_actionDashboardContentsChanged=actionOnDashboardContentsChanged;
			_actionDashboardClosing=actionOnDashboardClosing;
			//Failed to open any of the previously attached widgets, likely due to user no longer having permissions to widgets.
			//Continue if initializing Dashboard for the first time so that a widget can be added later.
			if(ListOpenWidgets.Count==0) {
				CloseDashboard(false);
				return;
			}
			IsInitialized=true;//Subscribes to relevant events.
			_actionDashboardContentsChanged?.Invoke();//Only after the Patient Dashboard SheetDef is added.
			StartRefreshThread();
		}

		private void PatientEvent_Fired(ODEventArgs e) {
			if(((e.Tag as Patient)?.PatNum??-1)==FormOpenDental.CurPatNum) {
				RefreshDashboard();
			}
		}

		private void PatientChangedEvent_Fired(ODEventArgs e) {
			RefreshDashboard();
		}

		private void PatientDashboardDataEvent_Fired(ODEventArgs e) {
			if(e==null || e.Tag==null) {
				return;
			}
			SetData(e);
		}

		private void UserControlDashboardWidgetRefresh_Fired(UserControlDashboardWidget sender,EventArgs e) {
			ODProgress.ShowAction(() => RefreshDashboard(),"Refreshing Dashboard.");
		}

		///<summary>Causes the refresh thread to either start, or wakeup and refresh the data in the background and refresh the UI on the main thread.
		///</summary>
		public void RefreshDashboard() {
			StartRefreshThread();
		}

		private void StartRefreshThread() {
			StartRefreshThread((widget) => widget.TryRefreshData(),REFRESH_THREAD_NAME);
		}

		private void StartRefreshThread(Func<UserControlDashboardWidget,bool> funcData,string name) {
			ODThread previousThread=null;
			if(_threadRefresh!=null && !_threadRefresh.HasQuit) {
				previousThread=_threadRefresh;
			}
			_threadRefresh=new ODThread((o) => {
				if(previousThread!=null) {
					//Allow the previous thread to finish updating its data, but quit before updating UI if it hasn't started yet.
					previousThread.QuitSync(Timeout.Infinite);
				}
				foreach(UserControlDashboardWidget widget in ListOpenWidgets) {
					if(funcData(widget)) {
						if(widget.IsDisposed || !widget.IsHandleCreated) {
							continue;
						}
						if(!o.HasQuit) {
							widget.RefreshView();//Invokes back to UI thread for UI update.
						}
					}
				}
			});
			_threadRefresh.Name=name;
			_threadRefresh.AddExceptionHandler(ex => ex.DoNothing());//Don't crash program on Dashboard data fetching failure.
			_threadRefresh.Start();
		}

		private void SetData(ODEventArgs e) {
			PatientDashboardDataEventArgs data;
			if(e.Tag is PatientDashboardDataEventArgs){
				data=(PatientDashboardDataEventArgs)e.Tag;
			}
			else{
				data=new PatientDashboardDataEventArgs(e.Tag);
			}
			StartRefreshThread((widget) => widget.TrySetData(data),SET_THREAD_NAME);
		}

		///<summary>Adds a new Widget to the current Dashboard container.</summary>
		public bool AddWidget(SheetDef sheetDefWidget) {
			if(sheetDefWidget==null || !Security.IsAuthorized(Permissions.DashboardWidget,sheetDefWidget.SheetDefNum,true)) {
				return false;
			}
			UserControlDashboardWidget widget=null;
			this.InvokeIfRequired(() => { 
				//Trying to open a widget that is already open.
				if(ListOpenWidgets.Any(x => x.SheetDefWidget.SheetDefNum==sheetDefWidget.SheetDefNum)) {
					return;
				}
				widget=new UserControlDashboardWidget(sheetDefWidget);
				widget.WidgetClosed+=CloseWidget;
				widget.RefreshClicked+=UserControlDashboardWidgetRefresh_Fired;
				widget.Anchor=((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left));
				if(!widget.IsHandleCreated) {
					IntPtr handle=widget.Handle;//Ensure the handle for this object is created on the Main thread.
				}
			});
			if(widget!=null && widget.Initialize()) {
				this.InvokeIfRequired(() => {
					widget.Location=new Point(0,0);
					widget.Anchor=(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
					Controls.Add(widget);
					widget.BringToFront();
				});
			}
			else {//Failed to open the widget, either due to lack of permission or failure to get the widget's SheetDef.
				return false;
			}
			_actionDashboardContentsChanged?.Invoke();
			return true;
		}

		private void menuItemClose_Click(object sender,EventArgs e) {
			CloseDashboard(false);
		}

		private void MenuItemRefresh_Click(object sender,EventArgs e) {
			UserControlDashboardWidgetRefresh_Fired((UserControlDashboardWidget)sender,e);
		}

		private void CloseWidget(UserControlDashboardWidget widget,EventArgs e) {
			if(widget==null) {
				return;
			}
			Controls.Remove(widget);
			if(ListOpenWidgets.Count==0) {
				IsInitialized=false;
				_actionDashboardContentsChanged?.Invoke();
				if(!_isLoggingOff) {
					_actionDashboardClosing?.Invoke();
				}
				_threadRefresh?.QuitSync(100);
			}
		}

		public void CloseDashboard(bool isLoggingOff) {
			if(InvokeRequired) {
				this.Invoke(() => { CloseDashboard(isLoggingOff); });
				return;
			}
			_isLoggingOff=isLoggingOff;
			if(ListOpenWidgets.Count==0) { //In case a dashboard manages to be left open with no widgets.
				IsInitialized=false;
				_actionDashboardContentsChanged?.Invoke();
				_actionDashboardClosing?.Invoke();
				_threadRefresh?.QuitSync(100);
			}
			for(int i=ListOpenWidgets.Count-1;i>=0;i--) {
				ListOpenWidgets[i].CloseWidget();
			}
			_isLoggingOff=false;
			//Clear these actions set during UserA's log on.  Otherwise, if UserB logs on and doesn't have a saved Dashboard, when UserB logs off, the 
			//actions defined for UserA (specifically, removing UserOdPrefs) will occur.
			_actionDashboardContentsChanged=null;
			_actionDashboardClosing=null;
		}
	}

}
