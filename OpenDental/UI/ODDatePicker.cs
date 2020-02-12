using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.UI {

	public partial class ODDatePicker:UserControl {

		public bool IsCalendarOpen;
		private DateTime _defaultDateTime=new DateTime(DateTime.Today.Year,1,1);
		///<summary>Event is fired when the calendar is opened.</summary>
		public event CalendarOpenedHandler CalendarOpened=null;
		///<summary>Event is fired when the calendar is closed.</summary>
		public event CalendarClosedHandler CalendarClosed=null;
		///<summary>Event is fired when either calendar has made a selection.</summary>
		public event CalendarSelectionHandler CalendarSelectionChanged=null;
		private bool _hideCalendarOnLeave=true;
		private bool _isParentChange;
		private Point _locationOrigCalendar;
		private CalendarLocationOptions _calendarLocation=CalendarLocationOptions.Below;
		///<summary>Hiding Control.Leave because the Leave event is fired whenever the user clicks on the calendar. This control will fire this Leave
		///event when the user truly leaves this control.</summary>
		public new event EventHandler Leave;
		///<summary>Event is fired with the text in textDate changes.</summary>
		public event DateTextChangedHandler DateTextChanged=null;
		///<summary>Adjustments to the calendar location.  Starting location is based on the CalendarLocation property.</summary>
		private Point _adjustCalendarLocation;

		#region Properties

		///<summary>Set location of calendar.</summary>
		[Browsable(true),DefaultValue(CalendarLocationOptions.Below),Description("Location where calendar appears relative to the date box.")]
		public CalendarLocationOptions CalendarLocation {
			get {
				return _calendarLocation;
			}
			set {
				_calendarLocation=value;
			}
		}
		
		///<summary>Adjust the position of the calendar.  Starting location is based on the CalendarLocation property.</summary>
		[Browsable(true),DefaultValue(typeof(Point),"0,0"),Description("Allows adustment of the calendar location.  Starting location is based on the CalendarLocation property.")]
		public Point AdjustCalendarLocation {
			get {
				return _adjustCalendarLocation;
			}
			set {
				_adjustCalendarLocation=value;
			}
		}

		///<summary>Set intial date</summary>
		[Category("Behavior"), Description("Set the initial date for load.")]
		public DateTime DefaultDateTime {
			get {
				return _defaultDateTime;
			}
			set {
				_defaultDateTime=value;
			}
		}

		///<summary>If true, will hide the calendar when focus leaves the control. If false, the calendar will stay open until the button is clicked.
		///</summary>
		[Category("Behavior"),DefaultValue(true),Description("Set whether the calendar will be hidden when leaving focus from the control.")]
		public bool HideCalendarOnLeave {
			get {
				return _hideCalendarOnLeave;
			}
			set {
				_hideCalendarOnLeave=value;
			}
		}

		#endregion

		public ODDatePicker() {
			InitializeComponent();
			SetDateTime(DefaultDateTime);
			base.Leave+=new System.EventHandler(this.ODDatePicker_Leave);
		}

		public void ChangeCalendarLocation() {
			switch(CalendarLocation) {
				case CalendarLocationOptions.Above:
					_locationOrigCalendar=new Point(1+_adjustCalendarLocation.X,textDate.Location.Y-1-calendar.Height+_adjustCalendarLocation.Y);
					break;
				case CalendarLocationOptions.ToTheRight:
					_locationOrigCalendar=new Point(textDate.Location.X+textDate.Width+1+_adjustCalendarLocation.X,textDate.Location.Y-1+_adjustCalendarLocation.Y);
					break;
				default://default to below
					_locationOrigCalendar=new Point(calendar.Location.X+_adjustCalendarLocation.X,calendar.Location.Y+_adjustCalendarLocation.Y);
					break;
			}
			calendar.Location=_locationOrigCalendar;
		}

		public DateTime GetDateTime() {
			return PIn.Date(textDate.Text);
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsValid {
			get {
				return textDate.IsValid;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int DateBoxWidth {
			get {
				return textDate.Width;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Point DateBoxLocation {
			get {
				return textDate.Location;
			}
		}

		public void SetDateTime(DateTime dateTime) {
			if(dateTime==DateTime.MinValue) {
				textDate.Text="";
			}
			else {
				textDate.Text=dateTime.ToShortDateString();
				calendar.SetDate(dateTime);
			}
		}
		
		///<summary>An empty string does not register as an error in ValidDate.</summary>
		public bool IsEmptyDateTime() {
			return (textDate.Text=="");
		}

		private void ODDatePicker_Load(object sender,EventArgs e) {
			ChangeCalendarLocation();
			textDate.Text=_defaultDateTime.ToShortDateString();
			HideCalendar();
			this.textDate.TextChanged+=new System.EventHandler(this.textDate_TextChanged);
		}
		
		private void butToggleCalendar_Click(object sender,EventArgs e) {
			ToggleCalendar();
		}

		public void ToggleCalendar() {
			if(calendar.Visible) {
				HideCalendar();
				CalendarClosed?.Invoke(this,new EventArgs());
				if(_isParentChange) {//Parent was not an ODForm, set back to original location and parent.
					calendar.Location=_locationOrigCalendar;
					calendar.Parent=this;
				}
			}
			else {
				ShowCalendar();
				CalendarOpened?.Invoke(this,new EventArgs());
			}
		}

		public void HideCalendar() {
			calendar.Visible=false;
			this.Height=this.MinimumSize.Height;
			IsCalendarOpen=false;
		}

		public void ShowCalendar() {
			//set the date on the calendar to match what's showing in the box
			if(IsValid) {
				if(textDate.Text=="") {
					//MonthCalendars have to have a selection, and Today's date seems to make the most sense,
					//even if implementors of this control interpret empty dates differently
					calendar.SetDate(DateTime.Today);
				}
				else {
					calendar.SetDate(PIn.Date(textDate.Text));
				}
			}
			if(!(this.Parent is ODForm)) {
				_isParentChange=true;
				Point locNew=calendar.Location;//Start from current context location.
				locNew=GetParentFormPoint(locNew,calendar);//Recursively work out to main form context.
				calendar.Location=locNew;//Set new location.
				calendar.Parent=Parent.FindForm();//Set new context.
				calendar.BringToFront();
			}
			//show the calendar
			calendar.Visible=true;
			IsCalendarOpen=true;
		}

		///<summary>Recursively calculates relative x-y coordinates up to this control's parent form.</summary>
		private Point GetParentFormPoint(Point locCur,Control contrCur) {
			if(contrCur.Parent==this.Parent.FindForm()) {
				return locCur;//Base case
			}
			locCur.Y+=contrCur.Parent.Location.Y;
			locCur.X+=contrCur.Parent.Location.X;
			return GetParentFormPoint(locCur,contrCur.Parent);
		}

		///<summary>Used to set the calendar date when the calendar is open</summary>
		public void SetCalendarDate(DateTime dateTime) {
			if(dateTime.Year < 1880) {//If they passed in a new date time
				calendar.SetDate(DateTime.Today);
			}
			else {
				calendar.SetDate(dateTime);
			}
		}

		private void calendar_DateSelected(object sender,DateRangeEventArgs e) {
			SetDateTime(calendar.SelectionStart);
			Validate();//if there was an error icon showing and they selected a valid date from the calendar, this will make the error icon go away
			if(calendar.Visible) {
				CalendarSelectionChanged?.Invoke(this,new EventArgs());
			}
		}

		private void textDate_TextChanged(object sender,EventArgs e) {
			DateTextChanged?.Invoke(sender,e);
		}

		private void ODDatePicker_Leave(object sender,EventArgs e) {
			if(calendar.Focused) {//Still using the calendar.
				this.Focus();//Spoof that we never left
				return;
			}
			if(HideCalendarOnLeave) {
				if(calendar.Visible) {
					HideCalendar();
					if(_isParentChange) {//Parent was not an ODForm, set back to original location and parent.
						calendar.Location=_locationOrigCalendar;
						calendar.Parent=this;
					}
					CalendarClosed?.Invoke(this,new EventArgs());
				}
			}
			Leave?.Invoke(sender,e);
		}

		public enum CalendarLocationOptions {
			Above,
			ToTheRight,
			Below,
		}
	}

	public delegate void CalendarClosedHandler(object sender,EventArgs e);

	public delegate void CalendarSelectionHandler(object sender,EventArgs e);

	public delegate void CalendarOpenedHandler(object sender,EventArgs e);

	public delegate void DateTextChangedHandler(object sender,EventArgs e);
}
