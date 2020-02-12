using CodeBase;
using OpenDentBusiness;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using OpenDental.UI;
using System.Drawing;
using Microsoft.VisualBasic.Devices;

namespace OpenDental {
	///<summary>Most OD forms extend this class. It does help and signal processing.</summary>
	public class ODForm : Form {

		///<summary>True when form has been shown by the system.
		///Shown occurs last in the forms construction life cycle.
		///The Shown event is only raised the first time a form is displayed.</summary>
		private bool _hasShown=false;
		///<summary>Only true if FormClosed has been called by the system.</summary>
		private bool _hasClosed=false;
		///<summary>Keeps track of window state changes.  We use it to restore minimized forms to their previous state.</summary>
		private FormWindowState _windowStateOld;
		private FormHelpWrapper _formHelp=null;
		///<summary>Holds the windows edition of the client. HelpForm and CreateHelpMenu() use this.
		///Linux users will be treated as Windows 10 as far as the HelpForm is concerned. Formatting
		///issues on Linux will have to be handled as they are found.</summary>
		private string _windowsEdition;
		///<summary>List of controls in the form that are used to filter something in the form.</summary>
		private List<Control> _listFilterControls=new List<Control>();
		///<summary>The given action to run after filter input is commited for FilterCommitMs.</summary>
		private Action _filterAction;
		///<summary>The thread that is ran to check if filter controls have had their changes commited.
		///If a single control is considered to have commited changes then the thread will only fire the _filterAction once and then will wait for more input.</summary>
		private ODThread _threadFilter;
		///<summary></summary>
		private DateTime _timeLastModified=DateTime.MaxValue;
		///<summary>The number of milliseconds to wait after the last user input on one of the specified filter controls to wait before calling _filterAction.
		///After some testing, 1 second felt most natural.</summary>
		protected int _filterCommitMs=1000;
		private static List<ODForm> _listSubscribedForms=new List<ODForm>();

		///<summary>Percentage across screen where help button is located. Scale is 0.0 to 1.0.</summary>
		public static double HelpButtonXAdjustment { get; protected set; }

		///<summary>Override and set to false if you want your form not to respond to F1 key for help.</summary>
		protected virtual bool HasHelpKey {
			get {
				return true;
			}
		}

		///<summary>True when form has been shown by the system.</summary>
		public bool HasShown {
			get {
				return _hasShown;
			}
		}

		///<summary>Only true if FormClosed has been called by the system.</summary>
		public bool HasClosed {
			get {
				return _hasClosed;
			}
		}

		public ODForm() {
			#region Designer Properties
			this.AutoScaleMode=AutoScaleMode.None;
			this.ClientSize=new System.Drawing.Size(974,696);
			this.KeyPreview=true;
			this.MinimumSize=new System.Drawing.Size(100,100);
			this.Name="ODForm";
			this.StartPosition=FormStartPosition.CenterScreen;
			this.Text="ODForm";
			#endregion
			this.Load+=ODForm_Load;
			this.Shown+=new EventHandler(this.ODForm_Shown);
			this.FormClosing+=new FormClosingEventHandler(this.ODForm_FormClosing);//Will fire first for all FormClosing events of this form.
			this.Resize+=new EventHandler(this.ODForm_Resize);
		}

		private void ODForm_Load(object sender,EventArgs e) {
			BackColor=ODColorTheme.FormBackColor;
			_windowsEdition="Windows 10";
			ODException.SwallowAnyException(() => { _windowsEdition=new ComputerInfo().OSFullName; });
		}

		private void ODForm_Shown(object sender,EventArgs e) {
			_hasShown=true;//Occurs after Load(...)
			this.FormClosed+=delegate {
				_listSubscribedForms.Remove(this);
			};
			_listSubscribedForms.Add(this);
			//This form has just invoked the "Shown" event which probably means it is important and needs to actually show to the user.
			//There are times in the application that a progress window (e.g. splash screen) will be showing to the user and a new form is trying to show.
			//Therefore, forcefully invoke "Activate" if there is a progress window currently on the screen.
			//Invoking Activate will cause the new form to show above the progress window (if TopMost=false) even though it is in another thread.
			if(ODProgress.FormProgressActive!=null) {
				this.Activate();
			}
			ShowHelpButtonSafe();
		}

		private void ODForm_Resize(object sender,EventArgs e) {
			if(WindowState!=FormWindowState.Minimized) {
				_windowStateOld=WindowState;
			}
		}

		public void Restore() {
			if(WindowState==FormWindowState.Minimized) {
				WindowState=_windowStateOld;
			}
		}

		///<summary>Sets the entire form into "read only" mode by disabling all controls on the form.
		///Pass in any controls that should say enabled (e.g. Cancel button). 
		///This can be used to stop users from clicking items they do not have permission for.</summary>
		public void DisableForm(params Control[] enabledControls) {
			foreach(Control ctrl in this.Controls) {
				if(enabledControls.Contains(ctrl)) {
					continue;
				}
				//Attempt to disable the control.
				try {
					ctrl.Enabled=false;
				}
				catch(Exception ex) {
					//Some controls do not support being disabled.  E.g. the WebBrowser control will throw an exception here.
					ex.DoNothing();
				}
			}
		}

		#region Signal Processing

		public void ProcessSignals(List<Signalod> listSignals) {
			Logger.LogAction("ODForm.ProcessSignals",LogPath.Signals,() => OnProcessSignals(listSignals),this.GetType().Name);
		}

		///<summary>Override this if your form cares about signal processing.</summary>
		public virtual void OnProcessSignals(List<Signalod> listSignals) {
		}

		///<summary>Spawns a new thread to retrieve new signals from the DB, update caches, and broadcast signals to all subscribed forms.</summary>
		public static void SignalsTick(Action onShutdown,Action<List<ODForm>,List<Signalod>> onProcess,Action onDone) {
			//No need to check RemotingRole; no call to db.
			Logger.LogToPath("",LogPath.Signals,LogPhase.Start);
			List<Signalod> listSignals=new List<Signalod>();
			ODThread threadRefreshSignals=new ODThread((o) => {
				//Get new signals from DB.
				Logger.LogToPath("RefreshTimed",LogPath.Signals,LogPhase.Start);
				listSignals=Signalods.RefreshTimed(Signalods.SignalLastRefreshed);
				Logger.LogToPath("RefreshTimed",LogPath.Signals,LogPhase.End);
				//Only update the time stamp with signals retreived from the DB. Do NOT use listLocalSignals to set timestamp.
				if(listSignals.Count>0) {
					Signalods.SignalLastRefreshed=listSignals.Max(x => x.SigDateTime);
					Signalods.ApptSignalLastRefreshed=Signalods.SignalLastRefreshed;
				}
				Logger.LogToPath("Found "+listSignals.Count.ToString()+" signals",LogPath.Signals,LogPhase.Unspecified);
				if(listSignals.Count==0) {
					return;
				}
				Logger.LogToPath("Signal count(s)",LogPath.Signals,LogPhase.Unspecified,string.Join(" - ",listSignals.GroupBy(x => x.IType).Select(x => x.Key.ToString()+": "+x.Count())));
				if(listSignals.Exists(x => x.IType==InvalidType.ShutDownNow)) {
					onShutdown();
					return;
				}
				InvalidType[] cacheRefreshArray = listSignals.FindAll(x => x.FKey==0 && x.FKeyType==KeyType.Undefined).Select(x => x.IType).Distinct().ToArray();
				//Always process signals for ClientDirect users regardless of where the RemoteRole source on the signal is from.
				//The middle tier server will have refreshed its cache already.
				bool getCacheFromDb=true;
				if(RemotingClient.RemotingRole==RemotingRole.ClientWeb
					&& !listSignals.Any(x => x.RemoteRole==RemotingRole.ClientDirect)) {
					//ClientWebs do not need to tell the middle tier to go to the database unless a ClientDirect has inserted a signal.
					getCacheFromDb=false;
				}
				Cache.Refresh(getCacheFromDb,cacheRefreshArray);
				onProcess(_listSubscribedForms,listSignals);
			});
			threadRefreshSignals.AddExceptionHandler((e) => {
				DateTime dateTimeRefreshed;
				try {
					//Signal processing should always use the server's time.
					dateTimeRefreshed=MiscData.GetNowDateTime();
				}
				catch {
					//If the server cannot be reached, we still need to move the signal processing forward so use local time as a fail-safe.
					dateTimeRefreshed=DateTime.Now;
				}
				Signalods.SignalLastRefreshed=dateTimeRefreshed;
				Signalods.ApptSignalLastRefreshed=dateTimeRefreshed;
			});
			threadRefreshSignals.AddExitHandler((o) => {
				Logger.LogToPath("",LogPath.Signals,LogPhase.End);
				onDone();
			});
			threadRefreshSignals.Name="SignalsTick";
			threadRefreshSignals.Start(true);
		}

		#endregion

		#region OD Help Logic

		///<summary>Takes the form's name and attempts to find a manual page associated to it to display to the user.</summary>
		///<param name="name">name of calling form</param>
		protected virtual void ShowHelp(string name) {
			string nameOverride=ShowHelpOverride();
			if(nameOverride!="") {
				name=nameOverride;
			}
			OpenDentalHelp.ODHelp.GetManualPage(name,PrefC.GetString(PrefName.ProgramVersion));
		}

		/// <summary>
		/// This method is used to hijack the form name being sent to ODHelp. If for some reason your form
		/// needs to send an alternative string as its name to ODHelp, override this method and set the return
		/// value as the name you want to send to ODHelp. See FormOpenDental as an example.
		/// </summary
		protected virtual string ShowHelpOverride() {
			return "";
		}

		///<summary>Creates or recreates the OD Help button. There are some spots in the program that close or modify all forms.
		///If that is the case the help button may have been affected and needs to be refreshed. Example- FormOpendental log off
		///closes all forms.</summary>
		public void ShowHelpButtonSafe() {
			ODException.SwallowAnyException(ShowHelpButton);
		}

		///<summary>This method creates the help button for all forms (unless turned off at the form level).
		///This is a hard-coded house of cards. If you modify this method you MUST test it on all of our supported 
		///Operating Systems.</summary>
		private void ShowHelpButton() {
			//Due to a Visual Studio bug, LicenseUsageMode.Designtime gives false positives when a form inherits ODForm.
			//The help button is too absolute when drawing itself in "Designtime" and will draw over Visual Studio itself and will stay there even after leaving the design file.
			//Therefore, only execute the Help Menu drawing logic when ODInitialize.Initialize() has been invoked.
			if(ODBuild.IsWeb()) {
				//Help button does not currently work with web viewed versions of open dental. Remove for now. 
				return;
			}
			if(!ODInitialize.HasInitialized) {
				return;
			}
			if(!HasHelpKey) {//Some forms might not want the help feature.
				return;
			}
			if(_formHelp!=null) {
				_formHelp.CloseForms();
			}
			double sizeEnhancer=1.3;
			int buttonWidth=(int)(SystemInformation.CaptionButtonSize.Width*sizeEnhancer);
			Rectangle screenRectangle=RectangleToScreen(this.ClientRectangle);
			int height=screenRectangle.Top-this.Top;
			if(this.Menu!=null) {
				//If this hardcoded value stops working we can call win32 methods to get the exact size at a performance cost.
				height-=20;
			}
			Size buttonSize=new Size(buttonWidth,height);
			_formHelp=new FormHelpWrapper(_windowsEdition,buttonSize);
			EventHandler handler=(o1,e1) => {
				bool hasMinMaxButtons=false;
				var thisRect=this.RectangleToScreen(this.DisplayRectangle);
				//The x coordinate to place our HelpForm. We find this by starting at x, adding the width of the form
				//(to get to the right hand side) and subtracting the sizes of the various buttons that can be shown in the titlebar.
				int x=thisRect.X+thisRect.Width;
				//Subtract ? button width
				x-=buttonSize.Width;
				if(ControlBox && (MaximizeBox || MinimizeBox)){//X, Minimize, and Maximize are all set to show
					hasMinMaxButtons=true;
					//Subtract X button size
					x-=buttonSize.Width;
					//Subtract size of maximize and minimize. If one is present they both are.
					x-=(buttonSize.Width)*2;
				}
				else if(ControlBox){//Just X button is shown
					x-=buttonSize.Width;
				}
				//Adjust for the current operating system.
				x+=GetHelpButtonXLocationAdjustment(hasMinMaxButtons);
				//Default placement, next to minimize or exit.
				_formHelp.DefaultStartPt=new Point(x,0); //Y doesn't matter here, but we need it to utilize PointTo methods
				//If user has changed help starting location, calculate new starting point.
				if(HelpButtonXAdjustment!=0){
					//Computer pref is based on position of _formHelp in client (window), other help code is based on entire screen position.
					//The below code is calculating the relative position of the help menu given the parent window's current size. This allows
					//for the help menu to continue to show even when resized and moved.
					Point clientPosition=new Point((int)(PointToClient(_formHelp.DefaultStartPt).X*HelpButtonXAdjustment),0);
					Point screenPosition=PointToScreen(clientPosition);
					x=screenPosition.X;
				}
				if(!_formHelp.IsDisposed) {
					if(this.WindowState==FormWindowState.Maximized) {
						_formHelp.SetLocation(x,this.Location.Y+3);
					}
					else {
						_formHelp.SetLocation(x,this.Location.Y);
					}
					//This block hides the "?" form when it is resized past the parent form's dimensions.
					if(this.Location.X>_formHelp.Location.X) {
						_formHelp.IsVisible=false;
					}
					else if(!_formHelp.IsVisible) {
						ShowHelpButton();
					}
				}
			};
			this.Resize+=handler;
			this.Move+=(o1,e1) => {
				handler(o1,e1);
			};
			_formHelp.LoadHelpUI+=(o1,e1) => {
				handler(o1,e1);
			};
			_formHelp.ClickHelp+=(o1,e1) => {
				//Don't want to be able to click on help button while user is moving it.
				if(!_formHelp.IsMoveOn) {
					ShowHelp(this.Name);
				}
			};
			_formHelp.MouseDownHelp+=(o1,e1) => {
				if(e1.Button==MouseButtons.Left) {
					_formHelp.MouseDownLocation=e1.Location;
					//We only want to allow moving if the user has clicked the "Move" menuitem and has clicked to initiate their mouse drag.
					if(_formHelp.CanDragToMove) {
						_formHelp.IsMoveOn=true;
					}
				}
			};
			_formHelp.RightClickHelp+=(o1,e1) => {
				RightClickHelp();
			};
			_formHelp.MouseMoveHelp+=(o1,e1) => {
				if(_formHelp.IsMoveOn) {
					MoveHelpButton(o1,e1);
				}
			};
			_formHelp.MouseUpHelp+=(o1,e1) => {
				if(_formHelp.IsMoveOn) {
					UpdateHelpAdjustment();
				}
				_formHelp.IsMoveOn=false;
				_formHelp.CanDragToMove=false;
			};
			_formHelp.Show(this);
		}

		///<summary>Disposes and nullifies the forms that are used to comprise the help button.
		///The "help button" is not technically a control that is owned by the parent form thus is not included in the disposal of the parent form.
		///This method needs to get invoked separately so that the references to the help button are removed thus allowing the garbage collector to clean.
		///Without explicitly disposing of these forms the memory used by the parent form cannot be correctly freed thus causing a memory leak.</summary>
		private void DisposeHelpButton() {
			if(_formHelp==null) {
				return;//Nothing to do.
			}
			ODException.SwallowAnyException(() => {
				_formHelp.CloseForms();
				if(!_formHelp.IsDisposed) {
					_formHelp.DisposeForms();
				}
			});
		}

		/// <summary>Returns an int value representing how much to adjust the x position for the ODHelp
		/// button based off of testing with each version. ComputerInfo().OSFullName returns the 
		/// OS product name (edition). Here is a list of all Window's product names as of 2/21/19
		/// https://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions </summary>
		private int GetHelpButtonXLocationAdjustment(bool hasMinMaxButtons) {
			if(_windowsEdition.Contains("Windows 7")) {
				return hasMinMaxButtons ? 30 : 5;
			}
			else if(_windowsEdition.Contains("Windows 8")) {
				return hasMinMaxButtons ? 35 : -5;
			}
			else if(_windowsEdition.Contains("Microsoft Windows Server 2012")) {
				return hasMinMaxButtons ? -30 : -10;
			}
			else {//Treat everything else as Windows 10
				return 0;
			}
		}

		///<summary>Should only be called if IsMove==true. Allows help button to be moved within the bounds of the window.</summary>
		private void MoveHelpButton(object sender,MouseEventArgs e) {
			//Allows dragging of help button
			int mouseXLocation=e.X+_formHelp.Location.X-_formHelp.MouseDownLocation.X;
			if(_formHelp.Location.X!=mouseXLocation && mouseXLocation>=this.Left && mouseXLocation<=_formHelp.DefaultStartPt.X) {
				if(this.WindowState==FormWindowState.Maximized) {
					_formHelp.SetLocation(mouseXLocation,this.Location.Y+3);
				}
				else {
					_formHelp.SetLocation(mouseXLocation,this.Location.Y);
				}
			}
		}

		///<summary>Right click on the help button, pops up with a menu to move the button.</summary>
		private void RightClickHelp() {
			ContextMenu contextMenu=new ContextMenu();
			contextMenu.MenuItems.Add(new MenuItem(Lans.g("ODForm","Move"),(s,eArgs) => { _formHelp.CanDragToMove=true; }));
			contextMenu.MenuItems[0].Visible=true;
			//Open the menu at the exact spot the user clicked.
			contextMenu.Show(this,PointToClient(MousePosition));
		}

		///<summary>Sets global preference for x location of help button. Percentage away on toolbar. 1=100%=Default.
		///This will be saved to db once FormOpenDental closes at the end of this session.
		///Saving from this class instance was too chatty so we will hang onto it and save when this session ends.</summary>
		private void UpdateHelpAdjustment() {
			//Create percentage user moved help button away from default starting place.
			HelpButtonXAdjustment=(double)PointToClient(_formHelp.Location).X/(double)PointToClient(_formHelp.DefaultStartPt).X;
		}
		#endregion

		#region Filtering

		///<summary>Call before form is Shown. Adds the given controls to the list of filter controls.
		///We will loop through all the controls in the list to identify the first control that has had its filter change commited for FilterCommitMs.
		///Once a filter is commited, the filter action will be invoked and the thread will wait for the next filter change to start the thread again.
		///Controls which are not text-based will commit immediately and will not use a thread (ex checkboxes).</summary>
		///<param name="filterCommitMs">The number of milliseconds to wait after the last user input on one of the specified filter controls to wait before calling _filterAction.</param>
		protected void SetFilterControlsAndAction(Action action,int filterCommitMs,params Control[] arrayControls) {
			SetFilterControlsAndAction(action,arrayControls);
			_filterCommitMs=filterCommitMs;
		}

		///<summary>Call before form is Shown. Adds the given controls to the list of filter controls.
		///We will loop through all the controls in the list to identify the first control that has had its filter change commited for FilterCommitMs.
		///Once a filter is commited, the filter action will be invoked and the thread will wait for the next filter change to start the thread again.
		///Controls which are not text-based will commit immediately and will not use a thread (ex checkboxes).</summary>
		protected void SetFilterControlsAndAction(Action action,params Control[] arrayControls) {
			if(HasShown) {
				return;
			}
			_filterAction=action;
			foreach(Control control in arrayControls) {
				//Keep the following if/else block in alphabetical order to it is easy to see which controls are supported.
				if(control.GetType().IsSubclassOf(typeof(CheckBox)) || control.GetType()==typeof(CheckBox)) {
					CheckBox checkbox = (CheckBox)control;
					checkbox.CheckedChanged+=Control_FilterCommitImmediate;
				}
				else if(control.GetType().IsSubclassOf(typeof(ComboBox)) || control.GetType()==typeof(ComboBox)) {
					ComboBox comboBox = (ComboBox)control;
					comboBox.SelectionChangeCommitted+=Control_FilterCommitImmediate;
				}
				else if(control.GetType().IsSubclassOf(typeof(ComboBoxMulti)) || control.GetType()==typeof(ComboBoxMulti)) {
					ComboBoxMulti comboBoxMulti = (ComboBoxMulti)control;
					comboBoxMulti.SelectionChangeCommitted+=Control_FilterCommitImmediate;
				}
				else if(control.GetType().IsSubclassOf(typeof(ODDateRangePicker)) || control.GetType()==typeof(ODDateRangePicker)) {
					ODDateRangePicker dateRangePicker = (ODDateRangePicker)control;
					dateRangePicker.CalendarSelectionChanged+=Control_FilterCommitImmediate;
				}
				else if(control.GetType().IsSubclassOf(typeof(ODDatePicker)) || control.GetType()==typeof(ODDatePicker)) {
					ODDatePicker datePicker = (ODDatePicker)control;
					datePicker.DateTextChanged+=Control_FilterChange;
				}
				else if(control.GetType().IsSubclassOf(typeof(TextBoxBase)) || control.GetType()==typeof(TextBoxBase)) {
					//This includes TextBox and RichTextBox, therefore also includes ODtextBox, ValidNum, ValidNumber, ValidDouble.
					control.TextChanged+=Control_FilterChange;
				}
				else if(control.GetType().IsSubclassOf(typeof(ListBox)) || control.GetType()==typeof(ListBox)) {
					control.MouseUp+=Control_FilterChange;
				}
				else if(control.GetType().IsSubclassOf(typeof(ComboBoxClinicPicker)) || control.GetType()==typeof(ComboBoxClinicPicker)) {
					((ComboBoxClinicPicker)control).SelectionChangeCommitted+=Control_FilterCommitImmediate;
				}
				else if(control.GetType().IsSubclassOf(typeof(ComboBoxPlus)) || control.GetType()==typeof(ComboBoxPlus)) {
					((ComboBoxPlus)control).SelectionChangeCommitted+=Control_FilterCommitImmediate;
				}
				else {
					throw new NotImplementedException("Filter control of type "+control.GetType().Name+" is undefined.  Define it in ODForm.AddFilterControl().");
				}
				_listFilterControls.Add(control);
			}
		}
		
		///<summary>A typical try-get, with an additional check to see if the form is disposed or control is disposed.</summary>
		private bool TryGetFilterInfo(Control control) {
			if(this.Disposing || this.IsDisposed || control.IsDisposed) {
				return false;
			}
			 return true;
		}

		///<summary>Commits the filter action immediately.</summary>
		private void Control_FilterCommitImmediate(object sender,EventArgs e) {
			if(!HasShown) {
				//Form has not finished the Load(...) function.
				//Odds are the form is initializing a filter in the form load and the TextChanged, CheckChanged, etc fired prematurely.
				return;
			}
			_timeLastModified=DateTime.Now;
			FilterActionCommit();//Immediately commit checkbox changes.
		}

		///<summary>Commits the filter action according to the delayed interval and input wakeup algorithm which uses FilterCommitMs.</summary>
		private void Control_FilterChange(object sender,EventArgs e) {
			if(!HasShown) {
				//Form has not finished the Load(...) function.
				//Odds are the form is initializing a filter in the form load and the TextChanged, CheckChanged, etc fired prematurely.
				return;
			}
			Control control=(Control)sender;
			if(!TryGetFilterInfo(control)) {
				return;
			}
			if(IsDisposedOrClosed(this)) {
				//FormClosed even has already occurred.  Can happen if a control in _listFilterControls has a filter action subscribed to an event that occurs after the 
				//FormClosed event, ex CellLeave in FormQueryParser triggers TextBox.TextChanged when closing via shortcut keys (Alt+O).
				return;
			}
			_timeLastModified=DateTime.Now;
			if(_threadFilter==null) {//Ignore if we are already running the thread to perform a refresh.
				//The thread does not ever run in a form where the user has not modified the filters.
				#region Init _threadFilter      
				this.FormClosed+=new FormClosedEventHandler(this.ODForm_FormClosed); //Wait until closed event so inheritor has a chance to cancel closing event.
				//No need to add thread waiting. We will take care of this with FilterCommitMs within our own thread when it runs.
				_threadFilter=new ODThread(1,((t)=> { ThreadCheckFilterChangeCommited(t); })); 
				_threadFilter.Name="ODFormFilterThread_"+Name;
				//Do not add an exception handler here. It would inadvertantly swallow real exceptions as thrown by the Main thread.
				_threadFilter.Start(false);//We will quit the thread ourselves so we can track other variables.
				#endregion
			}
			else {
				_threadFilter.Wakeup();
			}
		}
		
		///<summary>The thread belonging to Control_FilterChange.</summary>
		private void ThreadCheckFilterChangeCommited(ODThread thread) {
			//Might be running after FormClosing()
			foreach(Control control in _listFilterControls) {
				if(thread.HasQuit) {//In case the thread is executing when the user closes the form and QuitSync() is called in FormClosing().
					return;
				}
				if(!TryGetFilterInfo(control)) {//Just in case.
					continue;
				}
				double diff=(DateTime.Now-_timeLastModified).TotalMilliseconds;
				if(diff<=_filterCommitMs) {//Time elapsed is less than specified time.
					continue;//Check again later.
				}
				FilterActionCommit();
				thread.Wait(int.MaxValue);//Wait forever... or until Control_FilterChange(...) wakes the thread up or until form is closed.
				break;//Do not check other controls since we just called the filters action.
			}
		}

		private void FilterActionCommit() {
			Exception ex=null;
			//Synchronously invoke the "Refresh"/filter action function for the form on the main thread and invoke to prevent thread access violation exceptions.
			this.InvokeIfNotDisposed(()=> {
				//Only invoke if action handler has been set.
				try {
					_filterAction?.Invoke();
				}
				catch(Exception e) {
					//Simply throwing here would replace the stack trace with this thread's stack. 
					//Provide this exception as the inner exception below once we are out of the main thread's invoke to preserve both.
					ex=e;
				}
			});
			//Throw any errors that happened within the worker delegate while we were in a threaded context.
			ODException.TryThrowPreservedCallstack(ex);
		}

		#endregion Filtering

		[Obsolete]
		///<summary>Goes through all the controls on the form that implement IValid and shows a message box and returns false if any are not valid.</summary>
		protected bool ValidateInput() {
			if(this.GetAllControls().OfType<IValid>().Any(x => !x.IsValid)) {
				MsgBox.Show(this,"Please fix data errors first.");
				return false;
			}
			return true;
		}

		///<summary>Returns true if the form passed in has been disposed or if it extends ODForm and HasClosed is true.</summary>
		public static bool IsDisposedOrClosed(Form form) {
			bool isClosed=false;
			if(form.IsDisposed) {//Usually the system will set IsDisposed to true after a form has closed.  Not true for FormHelpBrowser.
				isClosed=true;
			}
			else if(form.GetType().GetProperty("HasClosed")!=null) {//Is a Form and has property HasClosed => Assume is an ODForm.
				//Difficult to compare type to ODForm, because it is a template class.
				if((bool)form.GetType().GetProperty("HasClosed").GetValue(form)) {//This is how we know FormHelpBrowser has closed.
					isClosed=true;
				}
			}
			return isClosed;
		}

		///<summary>Fires first for all FormClosing events of this form.</summary>
		private void ODForm_FormClosing(object sender,FormClosingEventArgs e) {
			//FormClosed event added to list of closing events as late as possible.
			//This allows the implementing form to set another FormClosing event to be fired before our base event here.
			//The advantage is that HasClosed will only be true if ALL FormClosing events have fired for this form.
			this.FormClosed+=new FormClosedEventHandler(this.ODForm_FormClosed);
		}

		private void ODForm_FormClosed(object sender,FormClosedEventArgs e) {
			if(_threadFilter!=null) {
				_threadFilter.QuitAsync();//It's fine if our thread loop finishes, it protects against unhandled exceptions.
				_threadFilter=null;
				//We don't want an enumeration exception here so don't clear _listFilterControls. It will get garbage collected anyways.
			}
			DisposeHelpButton();
			_hasClosed=true;
		}
	}

	///<summary>The class that encompasses the two forms used to create the help "button". This class 
	///reduces duplicate code. Simple eventing happens at this level. More complex events are tied to methods
	///after instantiation. If this class is modified please reconsider CreateHelpMenu().</summary>
	public class FormHelpWrapper {
		#region Properties and private data
		private FormHelpUI _formHelpUI;
		private FormHelpEvents _formHelpEvents;
		
		///<summary>Mouse down needed when user drags and moves mouse.</summary>
		public Point MouseDownLocation { get; set; }

		///<summary>A boolean that signals the user has clicked 'Move'.</summary>
		public bool CanDragToMove { get; set; }

		///<summary>Only true if CanDragToMove==true and the user has clicked the help form.</summary>
		public bool IsMoveOn { get; set; }

		///<summary>Starting pt if user hasnt moved help or changed their preference.</summary>
		public Point DefaultStartPt { get; set; }

		public bool IsDisposed {
			get {
				return (_formHelpUI==null || _formHelpEvents==null);
			}
		}

		public Size Size {
			set {
				_formHelpUI.Size=value;
				_formHelpEvents.Size=value;
			}
		}
		public Point Location {
			get { 
				return _formHelpUI.Location;
			}
		}
		public bool IsVisible {
			get {
				return (_formHelpUI.Visible || _formHelpEvents.Visible);
			}
			set {
				if(value) {
					_formHelpUI.Visible=true;
					_formHelpEvents.Visible=true;
				}
				else {
					_formHelpUI.Visible=false;
					_formHelpEvents.Visible=false;
				}
			}
		}
		#endregion

		#region Events
		public event EventHandler LoadHelpUI;
		public event EventHandler ClickHelp;
		public event EventHandler RightClickHelp;
		public event EventHandler MouseUpHelp;
		public event MouseEventHandler MouseDownHelp;
		public event MouseEventHandler MouseMoveHelp;
		#endregion

		#region Public methods
		public FormHelpWrapper(string osEdition,Size buttonSize) {
			_formHelpUI=new FormHelpUI(osEdition);// { Size=buttonSize };
			_formHelpEvents=new FormHelpEvents();// { Size=buttonSize };
			_formHelpUI.FormClosed+=(o1,e1) => {
				_formHelpUI=null;
			};
			_formHelpEvents.FormClosed+=(o1,e1) => {
				_formHelpEvents=null;
			};
			//For some reason _formHelpEvents resizes after Show() is called. This keeps it synched with _formHelpUI's size.
			_formHelpEvents.Shown+=(o1,e1) => {
				//_formHelpEvents.Size=_formHelpUI.Size;
				_formHelpEvents.Size=buttonSize;
				_formHelpUI.Size=buttonSize;
			};
			_formHelpUI.Load+=(o1,e1) => {
				LoadHelpUI?.Invoke(this,new EventArgs());
			};
			_formHelpEvents.MouseClick+=(o1,e1) => {
				if (e1.Button==MouseButtons.Right) {
					RightClickHelp?.Invoke(this,new EventArgs());
				}
				else {
					ClickHelp?.Invoke(this,new EventArgs());
				}
			};
			_formHelpEvents.MouseUp+=(o1,e1) => {
				MouseUpHelp?.Invoke(this,new EventArgs());
			};
			//MouseDown and MouseMove need to be MouseEventArgs so we can utilize mouse functionality (location, button pressed etc.)
			_formHelpEvents.MouseDown+=(o1,e1) => {
				MouseDownHelp?.Invoke(this,new MouseEventArgs(e1.Button,e1.Clicks,e1.X,e1.Y,e1.Delta));
			};
			_formHelpEvents.MouseMove+=(o1,e1) => {
				MouseMoveHelp?.Invoke(this,new MouseEventArgs(e1.Button,e1.Clicks,e1.X,e1.Y,e1.Delta));
			};
		}

		public void SetLocation(int x, int y) {
			_formHelpUI.Location=new Point(x,y);
			_formHelpEvents.Location=new Point(x,y);
		}

		public void Show(IWin32Window owner) {
			//Order matters here for z-index.
			_formHelpUI.Show(owner);
			_formHelpEvents.Show(owner);
		}

		///<summary>Close forms if they exist.</summary>
		public void CloseForms() {
			if(_formHelpUI!=null) {
				_formHelpUI.Close();
			}
			if(_formHelpEvents!=null) {
				_formHelpEvents.Close();
			}
		}

		///<summary>Disposes forms if they exist and have yet to be disposed.</summary>
		public void DisposeForms() {
			if(_formHelpUI!=null && !_formHelpUI.IsDisposed) {
				_formHelpUI.Dispose();
				_formHelpUI=null;
			}
			if(_formHelpEvents!=null) {
				_formHelpEvents.Dispose();
				_formHelpEvents=null;
			}
		}
		#endregion

		///<summary>HelpForm1 draws the text of the help "button". This form is 100% transparent.
		///This form handles the custom drawing and UI of the help "button".</summary>
		private class FormHelpUI : FormHelpBase {
			public string OsEdition;
			public FormHelpUI(string osEdition) {
				OsEdition=osEdition;
				BackColor=Color.Red;
				TransparencyKey=Color.Red;
				SetStyle(ControlStyles.ResizeRedraw,true);
			}

			protected override void OnPaint(PaintEventArgs e) {
				base.OnPaint(e);
				//Leave this here. Allows drawing on a form that is transparent.
				e.Graphics.TextRenderingHint=System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
				//Rectangle used by all drawing logic for positioning and sizing
				Rectangle rect=new Rectangle(new Point(0, 0), this.Size);
				//Create a smaller rectangle to use for drawing "?"
				rect.Inflate(-(rect.Width/4),-(rect.Height/4));
				//Get adjusted Y value for drawing based on OS
				int yAdjustment=0;
				if(OsEdition.Contains("Windows 7")) {
					yAdjustment+=3;
				}
				else if(OsEdition.Contains("Windows 8")) {
					yAdjustment-=2;
				}
				else {//treat everything else as Windows 10
					yAdjustment+=1;
				}
				using (SolidBrush brush=new SolidBrush(Color.FromArgb(255,80,80,80))) 
				using (Font font=new Font("Segoe UI",10,FontStyle.Bold))
				using(StringFormat format=new StringFormat() { Alignment=StringAlignment.Center, LineAlignment=StringAlignment.Center })
				{
					e.Graphics.DrawString("?",font,brush, new Rectangle(rect.X,rect.Y+yAdjustment,rect.Width,rect.Height),format);
				}
			}
		}

		///<summary>HelpForm2 is a blank form set to 99% opacity that lays on top of HelpForm1.
		///This form handles the eventing logic.</summary>
		private class FormHelpEvents : FormHelpBase {

			public bool IsHover {
				get {
					return Opacity==.5;
				}
				set {
					if(value) {
						this.Opacity=.5;
					}
					else {
						this.Opacity=.01;
					}
				}
			}

			public FormHelpEvents() {
				IsHover=false;
				this.MouseEnter+=(o1,e1) => {
					IsHover=true;
				};
				this.MouseLeave+=(o1,e1) => {
					IsHover=false;
				};
			}
		}

		private class FormHelpBase : Form {

			protected override bool ShowWithoutActivation {
				get {
					return true;
				}
			}

			public FormHelpBase() {
				MaximizeBox=false;
				MinimizeBox=false;
				ShowIcon=false;
				ControlBox=false;
				ShowInTaskbar=false;
				AutoSize=false;
				FormBorderStyle=FormBorderStyle.None;
				StartPosition=FormStartPosition.Manual;
			}
		}
	}

}