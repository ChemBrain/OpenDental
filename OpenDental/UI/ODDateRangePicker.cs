﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental.UI {

	public partial class ODDateRangePicker:UserControl {

		private bool _enableWeekButtons=true;
		private bool _isVertical=false;
		private DateTime _defaultDateTimeFrom=new DateTime(DateTime.Today.Year,1,1);
		private DateTime _defaultDateTimeTo=DateTime.Today;
		///<summary>Event is fired when either calendar is closed.</summary>
		public event CalendarClosedHandler CalendarClosed=null;
		///<summary>Event is fired when either calendar has made a selection.</summary>
		public event CalendarSelectionHandler CalendarSelectionChanged=null;

		#region Properties

		///<summary>Set true to enable butWeekPrevious and butWeekNext</summary>
		[Category("Behavior"), Description("Set true to enable butWeekPrevious and butWeekNext."), DefaultValue(true)]
		public bool EnableWeekButtons {
			get {
				return _enableWeekButtons;
			}
			set {
				_enableWeekButtons=value;
				panelWeek.Visible=_enableWeekButtons;
			}
		}

		///<summary>Set true to enable butWeekPrevious and butWeekNext</summary>
		[Category("Behavior"), Description("Set the initial from date for load.")]
		public DateTime DefaultDateTimeFrom {
			get {
				return _defaultDateTimeFrom;
			}
			set {
				_defaultDateTimeFrom=value;
			}
		}

		///<summary>Set true to enable butWeekPrevious and butWeekNext</summary>
		[Category("Behavior"), Description("Set the initial to date for load.")]
		public DateTime DefaultDateTimeTo {
			get {
				return _defaultDateTimeTo;
			}
			set {
				_defaultDateTimeTo=value;
			}
		}

		///<summary>Set true to enable butWeekPrevious and butWeekNext</summary>
		[Category("Behavior"), Description("Whether the Date Range Picker is vertical."),DefaultValue(false)]
		public bool IsVertical {
			get {
				return _isVertical;
			}
			set {
				if(!_isVertical && value==true) {
					ChangeToVertical();
				}
				_isVertical=value;
			}
		}
		#endregion

		public ODDateRangePicker() {
			InitializeComponent();
		}

		public DateTime GetDateTimeFrom() {
			return datePickerFrom.GetDateTime();
		}

		///<summary>Gets the end of the date that is entered.</summary>
		public DateTime GetDateTimeTo(bool isDefaultMaxDateT=false) {
			try {
				DateTime dateTime=datePickerTo.GetDateTime();
				if(dateTime==DateTime.MinValue && isDefaultMaxDateT) {
					return DateTime.MaxValue;
				}
				return dateTime.AddHours(23).AddMinutes(59).AddSeconds(59);
			}
			catch(Exception ex) {
				ex.DoNothing();
				if(isDefaultMaxDateT) {
					return DateTime.MaxValue;
				}
				return DateTime.MinValue;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsValid {
			get {
				return datePickerFrom.IsValid && datePickerTo.IsValid;
			}
		}
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsCalendarExpanded {
			get {
				if(_isVertical) {
					return (datePickerFrom.IsCalendarOpen || datePickerTo.IsCalendarOpen);
				}
				return (datePickerFrom.IsCalendarOpen && datePickerTo.IsCalendarOpen);
			}
		}

		public void SetDateTimeFrom(DateTime dateTime) {
			if(dateTime==DateTime.MinValue) {
				datePickerFrom.SetDateTime(DateTime.MinValue);
			}
			else {
				datePickerFrom.SetDateTime(dateTime);
				if(dateTime > GetDateTimeTo()) {
					SetDateTimeTo(dateTime);
				}
			}
		}

		public void SetDateTimeTo(DateTime dateTime) {
			if(dateTime==DateTime.MinValue) {
				datePickerTo.SetDateTime(DateTime.MinValue);
			}
			else {
				datePickerTo.SetDateTime(dateTime);
				if(dateTime < GetDateTimeFrom()) {
					SetDateTimeFrom(dateTime);
				}
			}
		}

		///<summary>An empty string does not register as an error in ValidDate.</summary>
		public bool HasEmptyDateTimeFrom() {
			return datePickerFrom.IsEmptyDateTime();
		}

		///<summary>An empty string does not register as an error in ValidDate.</summary>
		public bool HasEmptyDateTimeTo() {
			return datePickerTo.IsEmptyDateTime();
		}

		///<summary>Returns true if the passed in date is within the currently selected dates.</summary>
		public bool IsInDateRange(DateTime date) {
			return date.Between(GetDateTimeFrom(),GetDateTimeTo());
		}

		private void ODDateRangePicker_Load(object sender,EventArgs e) {
			if(!_enableWeekButtons) {
				panelWeek.Visible=false;
			}
			datePickerFrom.SetDateTime(_defaultDateTimeFrom);
			datePickerTo.SetDateTime(_defaultDateTimeTo);
			HideCalendars();
			TrySetAutoCloseControls();
		}

		///<summary>Changes the orientation to vertical. This change is after load and will not show in designer until built.
		///</summary>
		private void ChangeToVertical() {
			//Anchors need to change for the "To" elements, so that they are no longer pulled to the right when we resize below.
			datePickerTo.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			label2.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
			datePickerTo.Location=new Point(datePickerFrom.Location.X,datePickerFrom.Location.Y+datePickerFrom.Height-1);
			label2.Location=new Point(label1.Location.X+11,label1.Location.Y+(datePickerTo.Location.Y-datePickerFrom.Location.Y));
			panelWeek.Location=new Point(datePickerFrom.Location.X+(datePickerFrom.Width-panelWeek.Width),datePickerTo.Location.Y
				+datePickerTo.Height+1);
			datePickerTo.CalendarLocation=ODDatePicker.CalendarLocationOptions.ToTheRight;
			datePickerFrom.CalendarLocation=ODDatePicker.CalendarLocationOptions.ToTheRight;
			datePickerTo.ChangeCalendarLocation();
			datePickerFrom.ChangeCalendarLocation();
			datePickerTo.HideCalendarOnLeave=true;
			datePickerFrom.HideCalendarOnLeave=true;
			MinimumSize=new Size(datePickerFrom.DateBoxWidth+datePickerFrom.DateBoxLocation.X,datePickerTo.Location.Y+datePickerTo.Height);
			Size=new Size(panelWeek.Location.X+panelWeek.Width+3,panelWeek.Location.Y+panelWeek.Height+3);//3 pixels of extra padding
		}

		public void HideCalendars() {
			datePickerFrom.HideCalendar();
			datePickerTo.HideCalendar();
		}

		private void butWeekPrevious_Click(object sender,EventArgs e) {
			DateTime dateFrom=datePickerFrom.GetDateTime();
			DateTime dateTo=datePickerTo.GetDateTime();
			if(dateFrom.Year > 1880) {
				dateTo=dateFrom.AddDays(-1);
				datePickerFrom.SetDateTime(dateTo.AddDays(-7));
				datePickerTo.SetDateTime(dateTo);
			}
			else if(dateTo.Year > 1880) {//Invalid dateFrom but valid dateTo
				dateTo=dateTo.AddDays(-8);
				datePickerFrom.SetDateTime(dateTo.AddDays(-7));
				datePickerTo.SetDateTime(dateTo);
			}
			else {//Both dates invalid
				datePickerFrom.SetDateTime(DateTime.Today.AddDays(-7));
				datePickerTo.SetDateTime(DateTime.Today);
			}
			if(datePickerFrom.IsCalendarOpen) { //textTo and textFrom are set above, so no check is necessary.
				datePickerFrom.SetCalendarDate(datePickerFrom.GetDateTime());
				datePickerTo.SetCalendarDate(datePickerTo.GetDateTime());
			}
			CalendarSelectionChanged?.Invoke(this,new EventArgs());
		}

		private void butWeekNext_Click(object sender,EventArgs e) {
			DateTime dateFrom=datePickerFrom.GetDateTime();
			DateTime dateTo=datePickerTo.GetDateTime();
			if(dateTo.Year > 1880) {
				dateFrom=dateTo.AddDays(1);
				datePickerFrom.SetDateTime(dateFrom);
				datePickerTo.SetDateTime(dateFrom.AddDays(7));
			}
			else if(dateFrom.Year > 1880) {//Invalid dateTo but valid dateFrom
				dateFrom=dateFrom.AddDays(8);
				datePickerFrom.SetDateTime(dateFrom);
				datePickerTo.SetDateTime(dateFrom.AddDays(7));
			}
			else {//Both dates invalid
				datePickerFrom.SetDateTime(DateTime.Today);
				datePickerTo.SetDateTime(DateTime.Today.AddDays(7));
			}
			if(datePickerFrom.IsCalendarOpen) { //textTo and textFrom are set above, so no check is necessary.
				datePickerFrom.SetCalendarDate(datePickerFrom.GetDateTime());
				datePickerTo.SetCalendarDate(datePickerTo.GetDateTime());
			}
			CalendarSelectionChanged?.Invoke(this,new EventArgs());
		}

		private void datePickerFrom_CalendarSelectionChanged(object sender,EventArgs e) {
			if(datePickerFrom.GetDateTime() > datePickerTo.GetDateTime()) {
				datePickerTo.SetDateTime(datePickerFrom.GetDateTime());
			}
			CalendarSelectionChanged?.Invoke(this, new EventArgs());
		}

		private void datePickerTo_CalendarSelectionChanged(object sender,EventArgs e) {
			if(datePickerFrom.GetDateTime() > datePickerTo.GetDateTime()) {
				datePickerFrom.SetDateTime(datePickerTo.GetDateTime());
			}
			CalendarSelectionChanged?.Invoke(this,new EventArgs());
		}

		private void datePickerFrom_CalendarOpened(object sender,EventArgs e) {
			if(_isVertical) {//Show one calendar at a time
				if(datePickerTo.IsCalendarOpen) {
					datePickerTo.HideCalendar();
				}
			}
			else {//Otherwise, open them both at the same time
				datePickerTo.ShowCalendar();
			}
		}

		private void datePickerFrom_CalendarClosed(object sender,EventArgs e) {
			if(!_isVertical) {
				datePickerTo.HideCalendar();
			}
			CalendarClosed?.Invoke(this,new EventArgs());
		}

		private void datePickerTo_CalendarOpened(object sender,EventArgs e) {
			if(_isVertical) {//Show one calendar at a time
				if(datePickerFrom.IsCalendarOpen) {
					datePickerFrom.HideCalendar();
				}
			}
			else {//Otherwise, open them both at the same time
				datePickerFrom.ShowCalendar();
			}
		}

		private void datePickerTo_CalendarClosed(object sender,EventArgs e) {
			if(!_isVertical) {
				datePickerFrom.HideCalendar();
			}
			CalendarClosed?.Invoke(this,new EventArgs());
		}

		private void TrySetAutoCloseControls() {
			if(this.ParentForm==null) {
				return;//Just in case
			}
			SetAutoCloseControls(this.ParentForm);
		}

		private void SetAutoCloseControls(params Control[] arrayControls) {
			foreach(Control control in arrayControls) {
				if(control==this || this.Controls.Contains(control)) {
					continue;
				}
				if(control.HasChildren) {
					SetAutoCloseControls(control.Controls.OfType<Control>().ToArray());
				}
				control.MouseUp+=Control_MouseUp;
			}
		}

		private void Control_MouseUp(object sender,EventArgs e) {
			HideCalendars();
		}
	}
}
