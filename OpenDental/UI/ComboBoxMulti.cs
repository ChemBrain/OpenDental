using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental.UI {
	public class ComboBoxMulti : UserControl {
		#region Fields
		private ArrayList _listItems;
		private TextBox textMain;
		private ComboMultiDelimiter _comboDelimiter;
		private IContainer components;
		private ODThread _threadCheckKeyboard=null;
		private System.Windows.Forms.Button butDrop;
		private static bool _isLeftMouseDown;
		private static bool _isShiftDown;
		private static bool _isCtrlDown;
		private Panel panel1;
		private ODGrid gridMain;
		private PopupWindow _popup=null;
		//Eventually we may want to set this.
		private const int _maxDropDownHeight=500;
		#endregion Fields

		#region Constructor
		public ComboBoxMulti() {
			//This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			_listItems=new ArrayList();
			ComboDelimiter=ComboMultiDelimiter.Comma;//Required because we specified a default value of true for the designer.
			if(!_doUsePopup) {
				gridMain.VScrollVisible=false;//Since this is experimental, don't do it this way for everyone.
			}
		}
		#endregion Constructor

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComboBoxMulti));
			this.textMain = new System.Windows.Forms.TextBox();
			this.butDrop = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textMain
			// 
			this.textMain.BackColor = System.Drawing.Color.White;
			this.textMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textMain.Location = new System.Drawing.Point(2, 4);
			this.textMain.Name = "textMain";
			this.textMain.ReadOnly = true;
			this.textMain.Size = new System.Drawing.Size(97, 13);
			this.textMain.TabIndex = 2;
			// 
			// butDrop
			// 
			this.butDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDrop.BackColor = System.Drawing.Color.White;
			this.butDrop.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.butDrop.FlatAppearance.BorderSize = 0;
			this.butDrop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
			this.butDrop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
			this.butDrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.butDrop.Image = global::OpenDental.Properties.Resources.downArrowWinForm;
			this.butDrop.Location = new System.Drawing.Point(102,1);
			this.butDrop.Name = "butDrop";
			this.butDrop.Size = new System.Drawing.Size(17, 19);
			this.butDrop.TabIndex = 4;
			this.butDrop.UseVisualStyleBackColor = true;
			this.butDrop.Click += new System.EventHandler(this.dropButton_Click);
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(120, 21);
			this.panel1.TabIndex = 5;
			this.panel1.BorderStyle = BorderStyle.FixedSingle;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.gridMain.AutoSize = false;
			this.gridMain.Location = new System.Drawing.Point(0, 18);
			this.gridMain.Name = "gridMain";
			this.gridMain.ColorSelectedRow = System.Drawing.SystemColors.MenuHighlight;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(120, 70);
			this.gridMain.TabIndex = 3;
			this.gridMain.Title = null;
			this.gridMain.TranslationName = "Misc";
			this.gridMain.HeadersVisible=false;
			this.gridMain.TitleVisible=false;
			this.gridMain.Visible = false;
			this.gridMain.Margin=new Padding(0);
			this.gridMain.Padding=new Padding(0);
			// 
			// ComboBoxMulti
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.butDrop);
			this.Controls.Add(this.textMain);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.gridMain);
			this.Name = "ComboBoxMulti";
			this.Size = new System.Drawing.Size(120, 21);
			this.Load += new System.EventHandler(this.ComboBoxMulti_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ComboBoxMulti_Layout);
			this.Leave += new System.EventHandler(this.ComboBoxMulti_Leave);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Component Designer generated code

		#region Properties

		/// <summary>The items to display in the combo box.</summary>
		[Category("Data"),Description("The text of the items to display in the dropdown section.")]
		public virtual ArrayList Items{
			get{
				return _listItems;
			}
			set{
				_listItems=value;
			}
		}

		///<summary>Gets a value indicating whether the combo box is displaying its drop-down portion.</summary>
		private bool DroppedDown{
			get{
				if(!_doUsePopup) {
					return gridMain.Visible;
				}
				if(_popup==null) {
					return false;
				}
				return _popup.IsDroppedDown;
			}
		}

		///<summary>True if the dropdown will be inside a popup, false if the dropdown will be added to the parent form. One way in which the popup
		///is superior is that the drop down will extend outside the form if necessary.</summary>
		private bool _doUsePopup {
			get {
				//For some unknown reason, this popup would cause the parent form to go behind other windows, even when the form was opened using ShowDialog.
				return !ODBuild.IsWeb();
			}
		}

		public List<object> ListSelectedItems {
			get {
				return ListSelectedIndices.Select(x => Items[x]).ToList();
			}
		}

		///<summary>The indices of selected items.  Mimics the ListSelectedIndices property.</summary>
		public virtual ArrayList SelectedIndices{
			get{
				return new ArrayList(ArraySelectedIndices);
			}
			set{
				ArraySelectedIndices=value.Cast<int>().ToArray();
			}
		}

		///<summary>The indices of selected items.  Mimics the SelectedIndices property.</summary>
		public List<int> ListSelectedIndices {
			get{
				return ArraySelectedIndices.ToList();
			}
		}

		public virtual int[] ArraySelectedIndices {
			get {
				SynchronizeGrid();
				return gridMain.SelectedIndices;
			}
			set {
				gridMain.SetSelected(false);
				gridMain.SetSelected(value,true);
			}
		}

		///<summary>Use commas instead of OR in the display when muliple selected.</summary>
		[Category("Appearance"),Description("Use commas instead of OR in the display when muliple selected.")]
		[DefaultValue(ComboMultiDelimiter.Comma)]
		public ComboMultiDelimiter ComboDelimiter{
			get{
				return _comboDelimiter;
			}
			set{
				_comboDelimiter=value;
			}
		}

		#endregion Properties

		#region Methods - Public
		///<summary>Fills the combo box with each value from the enum. Uses the Description of the enum value if present, and sets the tag of the item
		///to the enum value.</summary>
		public void FillWithEnum<T>() {
			foreach(T enumVal in Enum.GetValues(typeof(T)).Cast<T>()) {
				Items.Add(new ODBoxItem<T>((enumVal as Enum).GetDescription(),enumVal));
			}
		}

		///<summary>Returns the tags of the selected items. The items in the combo box must be ODBoxItems.</summary>
		public List<T> SelectedTags<T>() {
			SynchronizeGrid();
			return gridMain.SelectedTags<T>();
		}

		public void SetSelected(bool setToValue) {
			SynchronizeGrid();
			gridMain.SetSelected(setToValue);
			FillText();
		}

		///<summary></summary>
		public void SetSelected(int index,bool setToValue){
			SetSelectedHelper(setToValue,index);
		}

		public void SetSelected(bool setToValue,params int[] arrayIndexes) {
			SetSelectedHelper(setToValue,arrayIndexes);
		}

		private void SetSelectedHelper(bool setToValue,params int[] arrayIndexes) {
			SynchronizeGrid();
			gridMain.SetSelected(arrayIndexes,setToValue);
			FillText();
		}

		public void SelectedIndicesClear() {
			//No need to synchronize the grid because the selected indices are just going to be cleared out regardless.
			gridMain.SetSelected(false);
		}

		///<summary>Sets the items for the comboBox to the given items list.</summary>
		///<param name="fItemToString">Func that takes an object that is the same type as the ODBoxItems Tags and returns a string to be displayed for this item, i.e.
		///x => x.Abbr.</param>
		///<param name="fSelectItem">Optional func that takes an object that is the same type as the ODBoxItems Tags and returns a bool, i.e.
		///x => x.ClinicNum==0. Pass null if you don't want to set initial item(s) to be selected.</param>
		public void SetItems<T>(IEnumerable<T> items,Func<T,string> fItemToString=null,Func<T,bool> fSelectItem=null) {
			Items.Clear();
			fItemToString=fItemToString??(x => x.ToString());
			foreach(T item in items) {
				Items.Add(new ODBoxItem<T>(fItemToString(item),item));
			}
			if(fSelectItem!=null) {
				SetSelectedItem(fSelectItem);
			}
		}

		///<summary>Sets the selected item(s) that match the func passed in. Will only work if Items are ODBoxItems.</summary>
		///<param name="fSelectItem">A func that takes an object that is the same type as the ODBoxItems Tags and returns a bool, i.e.
		///x => x.ClinicNum==0.</param>
		public void SetSelectedItem<T>(Func<T,bool> fSelectItem) {
			for(int i=0;i<Items.Count;i++) {
				ODBoxItem<T> odBoxItem=Items[i] as ODBoxItem<T>;
				if(odBoxItem==null) {
					continue;
				}
				if(fSelectItem(odBoxItem.Tag)) {
					SetSelected(i,true);
				}
			}
		}
		#endregion Methods - Public

		#region Events - Private
		private void ComboBoxMulti_Load(object sender, System.EventArgs e) {
			FillText();
			if(ParentForm!=null) {
				ParentForm.FormClosing+=ComboBoxMulti_ParentFormClosing;
				//If the user drags the parent window to a new location, collapse the combobox if dropped down.
				//Dragging the parent window via the title bar does not call ComboBoxMulti_Leave without this extra line of code.
				ParentForm.Move+=ComboBoxMulti_Leave;
			}
		}

		private void ComboBoxMulti_Layout(object sender, System.Windows.Forms.LayoutEventArgs e) {
			textMain.Width=Width-21;
			panel1.Width=Width;
			gridMain.Width=Width;
		}

		private void ComboBoxMulti_Leave(object sender, System.EventArgs e) {
			if(DroppedDown) {
				if(!_doUsePopup && gridMain.ContainsFocus) {
					return;
				}
				DropDownToggle();
			}
		}

		private void dropButton_Click(object sender,EventArgs e) {
			DropDownToggle();
		}

		private void DropDownToggle() {
			if(DroppedDown){//DroppedDown, so we need to collapse.
				if(_doUsePopup) {
					_popup.Close();//This collapses the popup.  The popup is NOT disposed.
				}
				else {
					gridMain.Visible=false;
				}
				FillText();
				if(SelectionChangeCommitted!=null) {
					SelectionChangeCommitted(this,new EventArgs());
				}
			}
			else{//Not DroppedDown, so we need to DropDown.
				#region Popup setup
				//If the grid rows differ from the items count, refill the grid because something happened to the ArrayList that the control wasn't aware of.
				//For example, If a ComboBoxMulti never sets an item selected, the grid is never filled, even though the ArrayList has items in it.
				//Filling the grid when opening the dropdown matches the UI behavior found in regular combo boxes.
				SynchronizeGrid();
				Control parent=Parent;
				int x=this.Location.X;
				int y=this.Location.Y;
				while(parent!=null && !typeof(Form).IsAssignableFrom(parent.GetType())) {
					x+=parent.Location.X;
					y+=parent.Location.Y;
					parent=parent.Parent;
				}
				if(_doUsePopup) {
					_popup=new PopupWindow(gridMain);
					_popup.AutoClose=false;//This prevents the Alt key from collapsing the combobox and prevents other events from collapsing as well.
					_popup.Closed+=PopupWindow_Closed;
					_popup.Opened+=PopupWindow_Opened;
					#endregion Popup setup
					_popup.Show(ParentForm,new Point(x,y+this.Height));
				}
				else {
					int gridTop=y+this.Height;
					if(gridTop+gridMain.Height>=ParentForm.ClientSize.Height) {
						//The grid does not have enough room to fit on the form.
						if(y > ParentForm.ClientSize.Height-gridTop) {
							//If there's more room, put the grid above the combobox.
							gridTop=y-gridMain.Height;
							if(gridTop < 0) {
								//Not enough room, shrink the grid height.
								gridMain.Height=y;
								gridTop=0;
							}
						}
						else {
							//If there's not enough room to show the grid below, shrink the grid height.
							gridMain.Height=ParentForm.ClientSize.Height-gridTop;
						}
					}
					gridMain.Location=new Point(x,gridTop);
					gridMain.Anchor=this.Anchor;
					ParentForm.Controls.Add(gridMain);
					gridMain.Visible=true;
					gridMain.BringToFront();
				}
			}
			//The keyboard listener lifetime begins on first dropdown and ends when the control is disposed or parent form is closed.
			if(_threadCheckKeyboard==null) {
				//When DroppedDown, will update input flags for auto collapsing.
				_threadCheckKeyboard=new ODThread(100,WorkerThread_KeyboardListener);
				_threadCheckKeyboard.AddExceptionHandler((ex) => ex.DoNothing());
				_threadCheckKeyboard.Start();
			}
		}
		#endregion Events - Private

		#region Popup Events

		public void PopupWindow_Closed(object sender,ToolStripDropDownClosedEventArgs e) {
			_popup.IsDroppedDown=false;
		}

		public void PopupWindow_Opened(object sender,EventArgs e) {
			_popup.IsDroppedDown=true;
		}

		#endregion Popup Events

		#region Input Listening Thread

		private void WorkerThread_KeyboardListener(ODThread odThread) {
			if(!DroppedDown) {
				return;
			}
			try {
				Invoke(new DelegateKeyboardListener(UpdateInputStates));
			}
			catch(ObjectDisposedException ex) {
				if(this.IsDisposed) {
					//On rare occasion, the control is disposed near the same instant that it is Ivoked above.
					//In this situation, we are quiting the thread, thus we expect the control to be disposed and ignore the error.
					return;
				}
				throw ex;
			}
		}

		private void UpdateInputStates() {
			if(!_isLeftMouseDown //No pertinent inputs are being held down.
				&& !_isShiftDown
				&& !_isCtrlDown
				&& MouseOverGrid() //Not over gridMain.vScroll
				&& Control.MouseButtons.HasFlag(MouseButtons.Left))
			{
				_isLeftMouseDown=true;
			}
			else if(_isLeftMouseDown && !Control.MouseButtons.HasFlag(MouseButtons.Left)) {//User released left mouse key.
				_isLeftMouseDown=false;
				if(DroppedDown) {
					DropDownToggle();
				}
			}
			else if(!_isCtrlDown && Control.ModifierKeys.HasFlag(Keys.Control)) {//User just started holding the Ctrl key.
				_isCtrlDown=true;
				//Set flag so that gridMain maintains same behavior.
				//Typically you would set KeyPreview to true in a Form but we do not have that ability with UserControl.
				//See summary above ODGrid.Parent_KeyDown(...);
			}
			else if(_isCtrlDown && !Control.ModifierKeys.HasFlag(Keys.Control)) {//User released Ctrl key.
				_isCtrlDown=false;
				DropDownToggle();
			}
			else if(!_isShiftDown && Control.ModifierKeys.HasFlag(Keys.Shift)) {//User just started holding the Shift key.
				_isShiftDown=true;
			}
			else if(_isShiftDown && !Control.ModifierKeys.HasFlag(Keys.Shift) //User released the Shift key,
				&& !Control.ModifierKeys.HasFlag(Keys.Control))//and not holding Ctrl.
			{//User released the Shift key, and not holding Ctrl.
				_isShiftDown=false;
				DropDownToggle();
			}
		}

		///<summary>Returns true if mouse is over gridMain but no if it is over the vScrollBar.</summary>
		private bool MouseOverGrid() {
			//Possition of the mouse relative to gridMain.
			Point mouseGridCoordinates=gridMain.PointToClient(Cursor.Position);
			if(mouseGridCoordinates.X>gridMain.Width-SystemInformation.VerticalScrollBarWidth) {//Cursor is over gridMain.vScroll.
				return false;
			}
			return true;
		}

		#endregion Input Listening Thread

		#region Methods - Private
		private void FillGrid() {
			List <object> listSelectedTags=new List<object>();//This list is used to maintain previous selections.
			foreach(int index in gridMain.SelectedIndices) {
				listSelectedTags.Add(gridMain.ListGridRows[index].Tag);
			}
			gridMain.BeginUpdate();
			gridMain.ListGridRows.Clear();//Clears the selections also.
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn();
			gridMain.ListGridColumns.Add(col);
			GridRow row;
			for(int i=0;i<_listItems.Count;i++) {
				row=new GridRow();
				row.Tag=_listItems[i];
				row.Cells.Add(_listItems[i].ToString());
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<gridMain.ListGridRows.Count;i++) {
				if(listSelectedTags.Contains(gridMain.ListGridRows[i].Tag)) {
					gridMain.SetSelected(i,true);
				}
			}
			//Arbitrary padding: 1 (for upper grid border) +1 (for lower grid border) +1 (for toolbar magic space at top) = 3
			gridMain.Height=Math.Min((gridMain.ListGridRows.Sum(x => x.State.HeightMain) + 3),_maxDropDownHeight);
		}

		private void FillText() {
			textMain.Clear();
			string delimiter="";
			switch(_comboDelimiter) {
				case ComboMultiDelimiter.Comma:
					delimiter=", ";
				break;
				case ComboMultiDelimiter.OR:
					delimiter=" OR ";
				break;
				case ComboMultiDelimiter.None:
					if(ArraySelectedIndices.Length>1) {
						textMain.Text=Lan.g(this,"Multiple Selected");
						return;
					}//Otherwise prints either a blank or a single selection.
				break;
			}
			//Old logic was assuming that the tag had a string representation: textMain.Text+=_listItems[ArraySelectedIndices[i]];
			//Also, the text that displays in each cell of the grid is gotten from "Items[X].ToString()" so this preserves that behavior.
			textMain.Text=string.Join(delimiter,SelectedTags<object>().Select(x => x.ToString()));
		}

		///<summary>Synchronizes gridMain with Items if needed.  This is critical to do before displaying the grid to the user or before asking
		///the grid for the currently selected indices.</summary>
		private void SynchronizeGrid() {
			if(gridMain.ListGridRows.Count==Items.Count) {
				for(int i=0;i<gridMain.ListGridRows.Count;i++) {
					if(gridMain.ListGridRows[i].Tag!=_listItems[i]) {
						FillGrid();
						break;
					}
				}
			}
			else {
				FillGrid();
			}
		}
		#endregion Methods - Private

		#region Thread Quiting

		private void QuitThread() {
			if(_threadCheckKeyboard!=null) {
				_threadCheckKeyboard.QuitAsync();
				_threadCheckKeyboard=null;
			}
		}

		private void ComboBoxMulti_ParentFormClosing(object sender,EventArgs e) {
			QuitThread();
		}

		protected override void Dispose(bool disposing) {
			QuitThread();
			base.Dispose(disposing);
		}

		#endregion Thread Quiting

		#region Delegates

		public delegate void DelegateKeyboardListener();

		public delegate void SelectionChangeCommittedHandler(object sender,EventArgs e);

		///<summary>Occurs when one of the menu items is selected.  This line causes the event to show in the designer.</summary>
		public event SelectionChangeCommittedHandler SelectionChangeCommitted;

		#endregion Delegates

	}

	#region Outside of Class
	public enum ComboMultiDelimiter {
		///<summary>When used combo box will simply show 'Multiple Selected'.</summary>
		None,
		OR,
		Comma,
	}

	///<summary>We need this special class so that we have access to ToolStripDropDown.content.</summary>
	internal class PopupWindow:ToolStripDropDown {
		private Control _content=null;
		private ToolStripControlHost _host=null;
		public bool IsDroppedDown=false;

		public PopupWindow(Control content) {
			this.AutoSize=false;
			this.DoubleBuffered=true;
			this.ResizeRedraw=false;
			this._content=content;
			this._host=new System.Windows.Forms.ToolStripControlHost(content);
			this._host.AutoSize=false;
			this._host.Anchor=AnchorStyles.None;
			this.Size=new Size(content.Width,content.Height+1);
			this.MinimumSize=this.Size;
			this.MaximumSize=this.Size;
			this._content.Location=Point.Empty;
			this.Padding=new Padding(0);
			this.Margin=new Padding(0);
			this.Items.Add(this._host);
		}

	}
	#endregion Outside of Class
}
















