using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;

namespace OpenDental.UI {
	//Jordan is the only one allowed to edit this file.
	//One common use use of this control is as a filter for reports, etc.
	//There should be no problems with this use.  The user cannot "see" clinics that they shouldn't, which is great and very useful.
	//But we need to be very careful in the case of using this combobox as a picker for fields on an object.
	//Since clinics with no permission would be hidden, this could inadvertently cause changes in which clinics are linked.  
	//Also, if the "all" option really means "all", then the calling form must specifically test IsAllSelected. 
	//"All" should not be used when the intent is ClinicNum=0
	//If the calling form instead uses ListSelectedClinicNums when All is selected, that's very different, and will exclude clinics not showing due to no permission.
	//This control is currently used in about 50 places.  Most of these are at the top, in the filter section of reports.  Here are the exceptions:
	//FormEServiceSetup has 4 comboBoxes.  I understand clinicPickerEClipboard, but not the other 3, so I can't police those 3.
	//FormPatientAddAll has 5 comboBoxes.  They don't include All or Unassigned, and they are all totally harmless, without potential for bugs.
	//FormPharmacyEdit: Includes All and Unassigned. It's multiselect. Does not test "All".  Because it uses a synch mechanism, clinics that aren't showing aren't affected either way.
	//FormRxEdit: Does not include All.  Includes Unassigned.  Single select. On save, if no selection, then it doesn't change the existing clinicNum.  I assume, but have not thoroughly verified, that the user must have permission for the clinic of this patient in order to get into their Rx.  So, we shouldn't have to worry about the clinic for this Rx not showing in the combobox, or at least it would be extremely rare and not worth addressing because an immediate OK would not change the value.

	//Should be ready now for implementation in any of the following 56 locations (and others I missed):
	//FormAdjMulti(picker), FormAdjust, FormApptEdit, FormApptViewEdit, FormApptViews, FormAsapSetup, FormBilling, FormBillingDefaults, 
	//FormBillingOptions(multi-select and picker), FormClaimEdit,
	//FormClaimPayEdit, FormClaimPayList, FormClaimSend, FormClearinghouseEdit, FormClearinghouses, FormClockEventEdit, FormCloneAdd, FormDepositEdit,
	//FormDeposits, FormDoseSpotAssignClinicId, FormDunningEdit(picker), FormDunningSetup(picker), FormEhrPatientExports, FormEServicesECR, FormEtrans835(multi-select), 
	//FormFeeSchedTools(multi-select badly implemented),FormFeeSchedTools2, FormLabCases, FormMedLabs, FormOperatoryEdit(picker), FormOrthoAutoClaims, FormPatientEdit, 
	//FormPatientSelect(might need to be more powerful for HQ use), FormPayConnectSetup, FormPayment, FormPayPlan, FormPaySimpleSetup, FormPaySplitEdit,
	//FormPendingPayments, FormPodiumSetup, FormProcBroken, FormProcCodes(higher priority), FormProcEdit, FormProcEditAll(pickerbox), FormSchedule,
	//FormScheduleDayEdit, FormScheduleEdit, FormSheetDefDefaults, FormSmsTextMessaging, FormTimeCardManage, FormTransworldSetup, 
	//FormWebForms(multi and picker), FormWebFormsSetup(picker), FormWebSchedASAPHistory, FormXChargeSetup, FormXWebTransactions
	//Many of these Forms are obviously not urgent at all because the clinic gets assigned automatically and wouldn't be changed.
	//But this control is ready to drop into any of the above forms, should the need arise.  Some of the forms need more help than others.

	///<summary>A Clinic comboBox that can scale up to thousands of Clinics.  Also handles multi select.  Fills itself with a filtered list of clinics that include only clinics that the current user has permission to access.  Automatically handles Visibility, based on PrefC.HasClinicsEnabled.  Notice that you have no access to the list of clinics or to the indices, which are instead handled with ClinicNums.  See bottom of this file for usage examples.</summary>
	public partial class ComboBoxClinicPicker:UserControl {
		#region Fields - Private Constant 
		///<summary>Represents all clinics.  This is done with a dummy clinic added to the top of the list with a ClinicNum of -2.</summary>
		private const long CLINIC_NUM_ALL=-2;
		///<summary>HQ/unassigned/default/none clinic with ClinicNum=0. Sometimes this dummy is filled with info from pref table instead of clinic table.</summary>
		private const long CLINIC_NUM_UNASSIGNED=0;
		#endregion Fields - Private Constant 

		#region Fields - Private
		///<summary>Disposed</summary>
		private SolidBrush _brushBack=new SolidBrush(Color.FromArgb(240,240,240));//lighter than built-in color of 225
		///<summary>Disposed</summary>
		private SolidBrush _brushDisabledBack=new SolidBrush(Color.FromArgb(204,204,204));//copied built-in color
		///<summary>Disposed</summary>
		private SolidBrush _brushDisabledText=new SolidBrush(Color.FromArgb(109,109,109));
		///<summary>Disposed</summary>
		private SolidBrush _brushHover=new SolidBrush(Color.FromArgb(229,241,251));//light blue
		///<summary>Property backer.</summary>
		private bool _includeAll=false;
		///<summary>Property backer.</summary>
		private bool _includeUnassigned=false;
		///<summary>Property backer.</summary>
		private bool _forceShowUnassigned=false;
		///<summary>This is the part that comes up as a "list" to pick from.</summary>
		private FormComboPicker _formComboPicker;
		///<summary>Property backer.</summary>
		private string _hqDescription="Unassigned";
		///<summary>True if the mouse is over the "combobox", to turn it a blue color.</summary>
		private bool _isHover;
		///<summary>Not exposed as public property.</summary>
		private List<Clinic> _listClinics=new List<Clinic>();
		///<summary>Not exposed as public property. Must stay synched with _selectedIndex.</summary>
		private List<int> _listSelectedIndices=new List<int>();
		///<summary>Disposed</summary>
		private Pen _penArrow=new Pen(Color.FromArgb(20,20,20),1.5f);
		///<summary>Disposed</summary>
		private Pen _penHoverOutline=new Pen(Color.FromArgb(0,120,215));//blue
		///<summary>Disposed</summary>
		private Pen _penOutline=new Pen(Color.FromArgb(173,173,173),1);
		///<summary>If the SelectedClinicNum gets set to a clinic that's not in the list because the user has no permission, then this is where that info is stored.  If this is null, then it instead behaves normally.</summary>
		private Clinic _selectedClinicNoPermission;
		///<summary>Not exposed as public property. Must stay synched with _listSelectedIndices.</summary>
		private int _selectedIndex=-1;
		///<summary>Property backer.</summary>
		private bool _selectionModeMulti=false;
		///<summary>Property backer.</summary>
		private bool _showLabel=true;
		///<summary>As this combo is initialized, the user defaults to CurUser. Can be changed.</summary>
		private Userod _userod=Security.CurUser;
		/// <summary>If this gets changed, then all places where this combo is used must be slightly adjusted.  This is a design weakness of this control, so just don't change it.  Wide enough to handle both "Clinic" and "Clinics".</summary>
		private int _widthLabelArea=37;
		#endregion Fields - Private

		#region Constructor
		public ComboBoxClinicPicker() {
			InitializeComponent();
			Size=new Size(200,21);//same as default
			Name="comboClinic";//default that can be changed
			//FillClinics();//this must come after setting test mode
		}
		#endregion Constructor

		#region Events - Public Raise
		///<summary></summary>
		private void OnSelectionChangeCommitted(object sender,EventArgs e){
			SelectionChangeCommitted?.Invoke(sender,e);
		}
		[Category("OD"),Description("Occurs when user selects a Clinic from the drop-down list.")]
		public event EventHandler SelectionChangeCommitted;

		///<summary></summary>
		private void OnSelectedIndexChanged(object sender,EventArgs e){
			SelectedIndexChanged?.Invoke(sender,e);
		}
		[Category("OD"),Description("Try not to use this. The preferred technique is to use SelectionChangeCommitted to react to each user click. In contrast, this event will fire even if the selection programmatically changes.")]
		public event EventHandler SelectedIndexChanged;
		#endregion Events - Public Raise

		#region Events - Protected Overrides
		protected override void OnHandleDestroyed(EventArgs e){
			//prevents an orphaned form from hanging around
			base.OnHandleDestroyed(e);
			if(_formComboPicker!=null && !_formComboPicker.IsDisposed){
				_formComboPicker.Close();
				_formComboPicker.Dispose();
			}
		}

		protected override void OnLoad(EventArgs e){
			base.OnLoad(e);
			FillClinics();
			if(!DesignMode && !IsTestModeNoDb) {
				Visible=PrefC.HasClinicsEnabled;
			}		
		}

		protected override void OnMouseDown(MouseEventArgs e){
			base.OnMouseDown(e);
			if(ShowLabel && e.X<_widthLabelArea){
				return;
			}
			if(_selectedClinicNoPermission!=null){
				MsgBox.Show("Not allowed");
				return;
			}
			_formComboPicker=new FormComboPicker();
			_formComboPicker.FormClosing += _formComboPicker_FormClosing;
			_formComboPicker.ListStrings=_listClinics.Select(x => x.Abbr).ToList();
			_formComboPicker.ListAbbrevs=_listClinics.Select(x => x.Abbr).ToList();
			_formComboPicker.PointInitialUR=this.PointToScreen(new Point(this.Width,0));
			_formComboPicker.MinimumSize=new Size(15,15);
			if(ShowLabel){
				_formComboPicker.Width=this.Width-_widthLabelArea;
			}
			else{
				_formComboPicker.Width=this.Width;
			}
			//SelectedIndices and SelectedIndex effectively set each other
			if(SelectionModeMulti){
				_formComboPicker.IsMultiSelect=true;
				_formComboPicker.SelectedIndices=_listSelectedIndices;
			}
			else{
				_formComboPicker.SelectedIndex=_selectedIndex;
			}
			_formComboPicker.Show();
		}

		protected override void OnMouseLeave(EventArgs e){
			base.OnMouseLeave(e);
			_isHover=false;
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e){
			base.OnMouseMove(e);
			if(ShowLabel && e.X<_widthLabelArea){
				if(!_isHover){
					return;//to avoid repeated invalidation
				}
				_isHover=false;
			}
			else{
				if(_isHover){
					return;
				}
				_isHover=true;
			}
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e){
			//the label and comboBox that show in the designer are just fake placeholders.  This control draws its own label and "combobox".  
			Graphics g=e.Graphics;
			g.SmoothingMode=SmoothingMode.AntiAlias;
			Rectangle rectangleCombo=new Rectangle();
			if(ShowLabel){
				rectangleCombo.X=_widthLabelArea;
			}
			else{
				rectangleCombo.X=0;
			}
			rectangleCombo.Y=0;
			rectangleCombo.Width=this.Width-rectangleCombo.X-1;
			rectangleCombo.Height=21-1;//the minus one is so it's not touching the edge of the control and hiding the drawing
			if(Enabled){
				if(_isHover){
					g.FillRectangle(_brushHover,rectangleCombo);
					g.DrawRectangle(_penHoverOutline,rectangleCombo);
				}
				else{
					g.FillRectangle(_brushBack,rectangleCombo);
					g.DrawRectangle(_penOutline,rectangleCombo);
				}
			}
			else{
				g.FillRectangle(_brushDisabledBack,rectangleCombo);
				g.DrawRectangle(_penOutline,rectangleCombo);
			}
			//the down arrow, starting at the left
			g.DrawLine(_penArrow,Width-13,9,Width-9.5f,12);
			g.DrawLine(_penArrow,Width-9.5f,12,Width-6,9);
			RectangleF rectangleFString=new RectangleF();
			rectangleFString.X=rectangleCombo.X+2;
			rectangleFString.Y=rectangleCombo.Y+4;
			rectangleFString.Width=rectangleCombo.Width-2;
			rectangleFString.Height=rectangleCombo.Height-4;
			int widthMax=rectangleCombo.Width-15;
			//"label" always shows the same, whether enabled or not
			if(ShowLabel){
				if(SelectionModeMulti){//"Clinics" is a tight fit
					//float widthText=g.MeasureString(labelFake.Text,Font).Width;//the only two possibilities are Clinic and Clinics.  
					//Lang trans would mess things up unless short, and it's not currently supported
					g.DrawString(labelFake.Text,Font,Brushes.Black,-2,4);
				}
				else{
					g.DrawString(labelFake.Text,Font,Brushes.Black,2,4);
				}
			}
			if(Enabled){
				g.DrawString(GetDisplayText(widthMax),this.Font,Brushes.Black,rectangleFString);//in combobox
			}
			else{
				g.DrawString(GetDisplayText(widthMax),this.Font,_brushDisabledText,rectangleFString);//in combobox
			}
		}
		#endregion Events -  Protected Overrides

		#region Events -  Private
		private void _formComboPicker_FormClosing(object sender, FormClosingEventArgs e) {
			//This is designed to fire whether or not changed. For example, FormPatientAddAll.ComboClinic1 needs to change the others even if it doesn't change.
			//important to set both _selectedIndex and _listSelectedIndices
			//Unsubscribe this event handler from the _formComboPicker.FormClosing event so other calls to close _formComboPicker won't trigger this method
			//again, e.g. Application.DoEvents will trigger the _formComboPicker.Deactivate event which calls _formComboPicker.Close() and this code would
			//run twice if we stayed subscribed to the FormClosing event.
			_formComboPicker.FormClosing-=_formComboPicker_FormClosing;
			_selectedIndex=_formComboPicker.SelectedIndex;
			_listSelectedIndices=_formComboPicker.SelectedIndices;
			OnSelectionChangeCommitted(this,e);
			OnSelectedIndexChanged(this,e);
			Refresh();
			Application.DoEvents();
		}
		#endregion Events -  Private

		#region Properties - Public
		[Category("OD")]
		[Description("This will be set to true if we always need to show Unassigned/0, regardless of user permissions.")]
		[DefaultValue(false)]
		public bool ForceShowUnassigned {
			get {
				return _forceShowUnassigned;
			}
			set {
				_forceShowUnassigned=value;
				FillClinics();
			}
		}

		[Category("OD")]
		[Description("Set to true to include 'All' as a selection option. 'All' can sometimes (e.g. FormOperatories) be intended to included more clinics than are actually showing in list.")]
		[DefaultValue(false)]
		//This is browsable, unlike ComboBoxPlus.  It's because this comboBox is intentionally slightly more automated. 
		//You never have to load the clinics manually, so there's no logcical place in the code where you might also want to specify all.
		public bool IncludeAll {
			get {
				return _includeAll;
			}
			set {
				_includeAll=value;
				FillClinics();
			}
		}

		///<summary>Set to true to include 'Unassigned' as a selection option. The word 'Unassigned' can be changed with the HqDescription property.  This is ClinicNum=0.</summary>
		[Category("OD")]
		[Description("Set to true to include 'Unassigned' as a selection option. The word 'Unassigned' can be changed with the HqDescription property.  This is ClinicNum=0.")]
		[DefaultValue(false)]//yes, true might be slightly better default, but then this could not be a drop-in replacement for ComboBoxClinic without causing bugs -- many bugs.
		public bool IncludeUnassigned {
			get {
				return _includeUnassigned;
			}
			set {
				_includeUnassigned=value;
				FillClinics();
			}
		}

		///<summary>The display value for ClinicNum 0. Default is 'Unassigned', but might want 'Default', 'HQ', 'None', 'Practice', etc.  Do not specify 'All' here, because that is not accurate.  Only used when 'DoIncludeUnassigned'</summary>
		[Category("OD")]
		[Description("The display value for ClinicNum 0. Default is 'Unassigned', but might want 'Default', 'HQ', 'None', 'Practice', etc.  Do not specify 'All' here, because that is not accurate.  Only used when 'DoIncludeUnassigned'")]
		[DefaultValue("Unassigned")]
		public string HqDescription {
			get {
				return _hqDescription;
			}
			set {
				_hqDescription=value;
				FillClinics();
			}
		}

		///<summary>True if the special dummy 'All' option is selected (regardless of any other additional selections). All needs to have been added, first.  The intent of All can vary, and the processing logic would be in the calling form.  On start, setting All would be done manually, not automatically.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsAllSelected{
			get{
				return SelectedClinicNum==CLINIC_NUM_ALL;
			}
			set{
				SetSelectedClinicNum(CLINIC_NUM_ALL);//bypasses the Property that would not allow this externally.
			}
		}

		///<summary>True if we are testing and db connection makes no sense.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(false)]
		public bool IsTestModeNoDb { get; set; } = false;

		///<summary>True if the 'unassigned'/default/hq/none/all clinic with ClinicNum=0 is selected.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsUnassignedSelected {
			get{
				return SelectedClinicNum==CLINIC_NUM_UNASSIGNED;
			}
			set{
				SelectedClinicNum=CLINIC_NUM_UNASSIGNED;
			}
		}

		///<summary>True if SelectedIndex==-1.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsNothingSelected {
			get{
				return _selectedIndex==-1;//this could be true if _selectedClinicNoPermission was set to some value that user did not have permission for
			}
			set{
				//not going to check permission here because it's coming from code rather than user.
				_selectedClinicNoPermission=null;
				_selectedIndex=-1;
				_listSelectedIndices=new List<int>();
				OnSelectedIndexChanged(this,new EventArgs());
				Invalidate();
			}
		}

		///<summary>Also can used when IsMultiSelect=false.  In the case where "All" is selected (regardless of any other additional selection), this will return a list of all clinicNums in the list.  This is not technically the same as All clinics in the database, because some clinics might be hidden from this user.  If unassigned(=0) is in the list when All is selected, then it will be included here.  If the calling form wishes to instead test All, and use other logic, it should test IsAllSelected. When setting, this isn't as rigorous as setting SelectedClinicNum.  This property will simply skip setting any clinics that aren't present because of no permission, but it will not disable the control to prevent changes. If !PrefC.HasClinicsEnabled, then this will return an empty list.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<long> ListSelectedClinicNums {
			//This control only internally keeps track of selected indices, so there's no local field for this.
			//The local indices need to be converted to/from ClinicNums.
			get{
				if(!DesignMode && !IsTestModeNoDb) {
					if(!PrefC.HasClinicsEnabled) {
						return new List<long>(); //when clinics are turned off this control doesn't appear and selected clinics is always empty list
					}
				}
				List<long> listSelectedClinicNums=new List<long>();
				if(_selectedClinicNoPermission!=null){
					listSelectedClinicNums.Add(_selectedClinicNoPermission.ClinicNum);
					return listSelectedClinicNums;
				}
				if(_listSelectedIndices.Count==0) {
					return listSelectedClinicNums;
				}
				if(_includeAll && _listSelectedIndices.Contains(0)){
					//The "All" item was selected
					foreach(Clinic clinic in _listClinics){
						if(clinic.ClinicNum==CLINIC_NUM_ALL){
							continue;
						}
						//seems to include the "unassigned/default/hq/none/all" clinicNum=0, if present
						listSelectedClinicNums.Add(clinic.ClinicNum);
					}
					return listSelectedClinicNums;
				}
				foreach(int idx in _listSelectedIndices){
					listSelectedClinicNums.Add(_listClinics[idx].ClinicNum);
				}
				return listSelectedClinicNums;
			}
			set{
				_selectedClinicNoPermission=null;
				_listSelectedIndices=new List<int>();
				for(int i=0;i<_listClinics.Count;i++) {
					if(value.Contains(_listClinics[i].ClinicNum)) {
						_selectedIndex=i;//this only works when there is one, but it's still important to try to stay synched
						_listSelectedIndices.Add(i);
					}
				}
				OnSelectedIndexChanged(this,new EventArgs());
				Invalidate();
			}
		}

		///<summary>Getter returns -1 if no clinic is selected. The setter is special.  If you set it to a clinicNum that this user does not have permission for, then the combobox displays that and becomes read-only to prevent changing it. It will also remember such a clinicNum and return it back in subsequent get.  0 (Unassigned) can also be set from here if it's present. This is common if pulling zero from the database.  But if you need to manually set to 0 in other situations, you should use the property IsUnassignedSelected.  You are not allowed to manually set to -2 (All) or -1 (none) from here.  Instead, set IsAllSelected=true or IsNothingSelected=true.  On initial load, this control will automatically select the current clinic, which you can of course change by setting a different clinic.  For example, you must manually set All.</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long SelectedClinicNum {
			get {
				if(!DesignMode && !IsTestModeNoDb) {
					if(!PrefC.HasClinicsEnabled) {
						return 0; //when clinics are turned off, this control doesn't appear and the 0 clinic is implicitly always selected
					}
				}
				if(_selectedClinicNoPermission!=null){
					return _selectedClinicNoPermission.ClinicNum;
				}
				if(_selectedIndex==-1) {
					return -1;
				}
				return _listClinics[_selectedIndex].ClinicNum;
			}
			set {
				if(value==CLINIC_NUM_ALL){
					throw new ApplicationException("Clinic num cannot be set to -2.  Instead, use IsAllSelected.");
				}
				if(value==-1){
					throw new ApplicationException("Clinic num cannot be set to -1.  Instead, set IsNothingSelected.");
				}
				SetSelectedClinicNum(value);				
			}
		}

		[Category("OD")]
		[Description("Set true for multi-select, false for single-select.")]
		[DefaultValue(false)]
		public bool SelectionModeMulti{
			get{
				return _selectionModeMulti;
			}
			set{
				if(_selectionModeMulti==value){
					return;
				}
				_selectionModeMulti=value;
				if(_selectionModeMulti){
					labelFake.Text="Clinics";
				}
			}
		}

		///<summary></summary>
		[Category("OD")]
		[Description("Normally true to show label on left.  Set to false if not enough space and you want to do your own separate label.  Make sure to manually set visibility of that label, based on whether Clinic feature is turned on.")]
		[DefaultValue(true)]
		public bool ShowLabel {
			get {
				return _showLabel;
			}
			set {
				if(_showLabel==value){
					return;
				}
				_showLabel=value; 
				//if(_showLabel){
				//	labelFake.Visible=true;
				//}
				//else{
				//	labelFake.Visible=false;
				//}
				Invalidate();
			}
		}

		#endregion Properties - Public

		#region Properties - Protected
		protected override Size DefaultSize{
			get{
				return new Size(200,21);
			}
		}

		public override string Text { 
			get{
				int widthMax=Width-1-15;
				if(ShowLabel){
					widthMax-=_widthLabelArea;
				}
				return GetDisplayText(widthMax); 
			}
		}
		#endregion Properties - Protected

		#region Methods - Public
		///<summary>Returns empty string if no clinic is selected.  Can return "All" or the "Unassigned" override abbreviation.</summary>
		public string GetSelectedAbbr() {
			if(_selectedIndex==-1) {
				return "";
			}
			return _listClinics[_selectedIndex].Abbr;
		}

		///<summary>Returns null if no clinic selected.</summary>
		public Clinic GetSelectedClinic() {
			if(_selectedIndex==-1) {
				return null;
			}
			return _listClinics[_selectedIndex];
		}

		///<summary>Gets a string of all selected clinic Abbr's, separated by commas.  If "All" is selected, then it simply returns "All Clinics", which might not be techinically true.  If ListSelectedClinicNums was used instead of testing IsAllSelected, then it could only include clinics that user has permission for.</summary>
		public string GetStringSelectedClinics() {
			List<Clinic> listSelectedClinics=_listSelectedIndices.Select(x => _listClinics[x]).ToList();
			if(listSelectedClinics.Any(x => x.ClinicNum==CLINIC_NUM_ALL)) {
				return "All Clinics";
			}
			return string.Join(",",listSelectedClinics.Select(clinic => clinic.Abbr));
		}

		///<summary>Lets you change which user is used to load the allowed clinics.</summary>
		public void SetUser(Userod userod){
			_userod=userod;
			FillClinics();
		}

		///<summary>If the calling code set this combo to a clinic that the user does not have permission to, then the user will already be blocked from changing the clinic by clicking on this combo.  But if you want them to also be blocked from doing other things, use this method to see if the combo is currently giving them permission to change clinics, and then take action accordingly.</summary>
		public bool UserHasPermission(){
			if(_selectedClinicNoPermission==null){
				return true;// a normal situation
			}
			return false;//user does not have permission
		}
		#endregion Methods - Public

		#region Methods - Private
		///<summary>This runs on load and with certain property changes that would only change at initialization.  Performance hit should be very small. This also does an Invalidate so that the "combobox" will update.  This also automatically selects a reasonable initial clinic, usually the current clinic. </summary>
		private void FillClinics() {
			if(!IsTestModeNoDb){
				if(!Db.HasDatabaseConnection && !Security.IsUserLoggedIn) {
					return;
				}
			}
			try {
				if(!IsTestModeNoDb){
					if(!PrefC.HasClinicsEnabled) {//Already not visible after load handler.
						return;
					}
				}
				_listClinics.Clear();
				if(IncludeAll) {//Comes first
					_listClinics.Add(new Clinic {
						Abbr="All",
						Description="All",
						ClinicNum=CLINIC_NUM_ALL
					});
				}
				//Does not  guarantee that HQ clinic will be included. Only if user has permission to view it.
				List<Clinic> listClinicsForUser=null;
				if(IsTestModeNoDb){
					listClinicsForUser=new List<Clinic>();
					for(int i=0;i<40;i++){
						listClinicsForUser.Add(new Clinic{Abbr="Clinic"+i.ToString(),Description="Clinic"+i.ToString(),ClinicNum=i });
					}
					if(IncludeAll){
						listClinicsForUser.Add(new Clinic{Abbr=HqDescription,Description=HqDescription,ClinicNum=0 });
					}
				}
				else{
					listClinicsForUser=Clinics.GetForUserod(_userod,true,HqDescription);
				}
				//Add HQ clinic when necessary.
				Clinic clinicUnassigned=listClinicsForUser.Find(x => x.ClinicNum==CLINIC_NUM_UNASSIGNED);
				//unassigned is next
				if(_forceShowUnassigned){
					_listClinics.Add(new Clinic{Abbr=HqDescription,Description=HqDescription,ClinicNum=0 });
				}
				else if(IncludeUnassigned  && clinicUnassigned!=null) {
					_listClinics.Add(clinicUnassigned);
				}
				//then, the other items, except unassigned
				listClinicsForUser.RemoveAll(x => x.ClinicNum==CLINIC_NUM_UNASSIGNED);
				_listClinics.AddRange(listClinicsForUser);
				//Will already be ordered alphabetically if that pref was set.  Unfortunately, a restart is required for that pref.
				//Setting selected---------------------------------------------------------------------------------------------------------------
				if(Clinics.ClinicNum==0) {
					if(IncludeUnassigned) {
						SelectedClinicNum=CLINIC_NUM_UNASSIGNED;
					}
					else if(IncludeAll) {
						SetSelectedClinicNum(CLINIC_NUM_ALL);
					}
				}
				else {
					//if Security.CurUser.ClinicIsRestricted, there will be only one clinic in the list, and it will not include default (0).
					SelectedClinicNum=Clinics.ClinicNum;
				}
			}
			catch(Exception e){
				e.DoNothing();
			}
			Invalidate();
		}

		///<summary>If multiple items are selected, we string them together with commas.  But if the string is wider than widthMax, we instead show "Multiple".</summary>
		private string GetDisplayText(int widthMax){
			if(_listSelectedIndices.Count==0){
				return "";
			}
			if(_listSelectedIndices.Contains(0) && _listClinics[0].ClinicNum==CLINIC_NUM_ALL){
				return "All";
			}
			if(_selectedClinicNoPermission!=null){
				return _selectedClinicNoPermission.Abbr;
			}
			string str="";
			for(int i=0;i<_listSelectedIndices.Count;i++){
				//impossible: if(_listClinics[_listSelectedIndices[i]].ClinicNum==CLINIC_NUM_ALL){
				if(i>0){
					str+=",";
				}
				str+=_listClinics[_listSelectedIndices[i]].Abbr;//automatically handles CLINIC_NUM_UNASSIGNED
			}
			if(_listSelectedIndices.Count>1){
				if(TextRenderer.MeasureText(str,this.Font).Width>widthMax){
					return "Multiple";
				}
			}
			return str;
		}

		///<summary>This is used separately from the SelectedClinicNum setter in order to internally set to -2 without an exception.  But this doesn't work for -1.</summary>
		private void SetSelectedClinicNum(long value){
			int idx=_listClinics.FindIndex(x=>x.ClinicNum==value);
			if(idx==-1){
				if(value==-2){
					//user tried to set All, but that option is not available.  
					//Don't do anything
				}
				else{
					//this user does not have permission for the selected clinic
					_selectedIndex=-1;
					_listSelectedIndices=new List<int>();
					if(IsTestModeNoDb){
						_selectedClinicNoPermission=new Clinic{Abbr=value.ToString(), Description=value.ToString(),ClinicNum=value };
					}
					else{
						_selectedClinicNoPermission=Clinics.GetClinic(value);
						//could still possibly be null in a corrupted db.  In that case, we still need to be able to return what was set.
						if(_selectedClinicNoPermission==null){
							_selectedClinicNoPermission=new Clinic{Abbr=value.ToString(), Description=value.ToString(),ClinicNum=value };
						}
					}
				}
			}
			else{
				_selectedClinicNoPermission=null;
				_selectedIndex=idx;
				_listSelectedIndices=new List<int>(){ idx };
			}
			OnSelectedIndexChanged(this,new EventArgs());
			Invalidate();
		}
		#endregion Methods - Private

		
	}
}

//Common Pattern#1, Clinic as a field:---------------------------------------------------------------------------------
//In the UI, set these properties:
//   IncludeUnassigned=true
//   HqDescription="None" //depending on situation
//comboClinic.SelectedClinicNum=adj.ClinicNum;
//...
//adj.ClinicNum=comboClinic.SelectedClinicNum;
//
//Common Pattern#2, Clinic as report filter----------------------------------------------------------------------------
//In the UI, set these properties:
//   IncludeAll=true
//   IncludeUnassigned=true
//   SelectionModeMulti=true
//string stringDisplayClinics=comboClinics.GetStringSelectedClinics();
//List<long> listClinicNums=comboClinics.ListSelectedClinicNums;
//RunReport(listClinicNums,stringDisplayClinics);




