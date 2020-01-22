﻿using System;

namespace CodeBase {
	///<summary>Helper class to allow multiple areas of the program to subscribe to various events which they care about.</summary>
	public class ODEvent {
		///<summary>Occurs when any developer calls Fire().  Can happen from anywhere in the program.
		///Consumers of "global" ODEvents need to register for this handler because this will be the event that mainly gets fired.</summary>
		public static event ODEventHandler Fired;

		///<summary>Triggers the global Fired event to get called with the passed in arguments.</summary>
		public static void Fire(ODEventType odEventType,object tag) {
			Fired?.Invoke(new ODEventArgs(odEventType,tag));
		}
	}

	///<summary>Arguments specifically designed for use in ODEvent.</summary>
	public class ODEventArgs {
		private object _tag;
		private ODEventType _eventType=ODEventType.Undefined;

		///<summary>A generic object related to the event, such as a Commlog object.  Can be null.</summary>
		public object Tag {
			get {
				return _tag;
			}
		}

		///<summary>Used to uniquly identify this ODEvent for consumers.  And event type of Undefined will be treated as a generic ODEvent.</summary>
		public ODEventType EventType {
			get {
				return _eventType;
			}
		}

		///<summary>Used when an ODEvent is needed but no object is needed in the consuming class.</summary>
		public ODEventArgs(ODEventType eventType) : this(eventType,null) {
		}

		///<summary>Creates an ODEventArg with the specified ODEventType and Tag passed in that is designed to be Fired to a progress window.</summary>
		///<param name="eventType">Progress windows registered to this ODEventType will consume this event arg and act upon the tag accordingly.
		///An event type of Undefined will be treated as a generic ODEvent.</param>
		///<param name="tag">Tag can be set to anything that the consumer may need.  E.g. a string for FormProgressStatus to show to users.</param>
		public ODEventArgs(ODEventType eventType,object tag) {
			_tag=tag;
			_eventType=eventType;
		}
	}

	///<summary>Only used for ODEvent.  Not necessary to reference this delegate directly.</summary>
	public delegate void ODEventHandler(ODEventArgs e);

	///<summary>Progress windows will be monitoring for these specific event types.</summary>
	public enum ODEventType {
		///<summary>0 - The event type has not been set.  Treated as a generic ODEvent.</summary>
		Undefined,
		///<summary>1 - The program is shutting down.</summary>
		Shutdown,
		///<summary>2 - A database maintenance progress event.</summary>
		DatabaseMaint,
		///<summary>3 - These events will get fired sporadically throughout the FormOpenDental.DataValid_BecameInvalid process.</summary>
		Cache,
		///<summary>4 - Miscellaneous things like backing up and repairing the database.</summary>
		MiscData,
		///<summary>5 - Events that occur during the conver databases operations.</summary>
		ConvertDatabases,
		///<summary>6 - Events that occur during PrefL operations.</summary>
		PrefL,
		///<summary>7 - Events that occur during billing operations.</summary>
		Billing,
		///<summary>8 - Events that occur during clinic operations.</summary>
		Clinic,
		///<summary>9 - Events that occur during fee schedule operations.</summary>
		FeeSched,
		///<summary>10 - Events that occur during patient operations.</summary>
		Patient,
		///<summary>11 - Events that occur during bug submission operations.</summary>
		BugSubmission,
		///<summary>12 - Events that occur during eService operations.</summary>
		EServices,
		///<summary>13 - Events that occur during eTrans operations.</summary>
		Etrans,
		///<summary>14 - Events that occur during clearinghouse operations.</summary>
		Clearinghouse,
		///<summary>15 - Events that occur during provider operations.</summary>
		Provider,
		///<summary>16 - Events that occur during Images Module operations.</summary>
		ContrImages,
		///<summary>17 - Events that occurs when the commitem window should automatically save.</summary>
		CommItemSave,
		///<summary>18 - Events that occurs when the wiki edit window should automatically save.</summary>
		WikiSave,
		///<summary>19 - Events that occurs during data connection operations.</summary>
		DataConnection,
		///<summary>20 - Events that occurs when credentials have failed.</summary>
		ServiceCredentials,
		///<summary>21 - Events that occurs when FormProcNotBilled has invoked the GoTo action.</summary>
		FormProcNotBilled_GoTo,
		///<summary>22 - Events that occurs when FormClaimSend has invoked the GoTo action.</summary>
		FormClaimSend_GoTo,
		///<summary>23 - (Deprecated) Events that occurs during the loading of FormOpenDental while the splash screen is showing.</summary>
		SplashScreenProgress,
		///<summary>24 - Events that occurs when the Email Message Edit window should automatically save.</summary>
		EmailSave,
		///<summary>25 - Events that occur during recall sync operations.</summary>
		RecallSync,
		///<summary>26 - Events that occur during report complex operations.</summary>
		ReportComplex,
		///<summary>27 - Events that occur during schedule operations.</summary>
		Schedule,
		///<summary>28 - Event that occurs when the eRx browser window has closed.</summary>
		ErxBrowserClosed,
		///<summary>29 - Event that occurs when hiding unused fee schedules.</summary>
		HideUnusedFeeSchedules,
		///<summary>30 - Event that occurs within job related windows and events at HQ.</summary>
		Job,
		///<summary>31 - Event that occurs when filling the Recall List grid.</summary>
		RecallList,
		///<summary>32 - Event that occurs when filling the Confirmation List grid.</summary>
		ConfirmationList,
		///<summary>33 - Event that occurs when filling the Planned Tracker grid.</summary>
		PlannedTracker,
		///<summary>34 - Event that occurs when filling the Unscheduled List grid.</summary>
		UnscheduledList,
		///<summary>35 - Event that occurs when filling the ASAP window list.</summary>
		ASAP,
		///<summary>36 - Event that occurs when filling the Insurance Verification list.</summary>
		InsVerification,
		///<summary>37 - Events that occurs during Middle Tier connection operations.</summary>
		MiddleTierConnection,
		///<summary>38 - Events that occurs during long ODGrid computations.</summary>
		ODGrid,
		///<summary>39 - Events that occurs when the user sends an appointment to the pinboard from DashApptGrid.</summary>
		SendToPinboard,
		///<summary>40 - Events that occurs when a crashed table has been detected.</summary>
		CrashedTable,
		///<summary>41 - Events that occur when a change has been made to an Appointment.</summary>
		AppointmentEdited,
		///<summary>42 - Events that occur when a Module is selected/refreshed.</summary>
		ModuleSelected,
		///<summary>43 - Events that occur when any eClipboard device experiences a status change.</summary>
		eClipboard,
		///<summary>44 - Events that occur when a query has started or finished executing.</summary>
		QueryMonitor,
		///<summary>45 - Events that cccur when unable to read from the MySQL data adapter.</summary>
		DataReaderNull,
		///<summary>46 - Events that occur during Userod operations.  e.g. changing users</summary>
		Userod,
	}
}
