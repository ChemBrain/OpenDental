using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public class FormSheetFieldBase:ODForm {

		///<summary>This is the object we are editing.</summary>
		public SheetFieldDef SheetFieldDefCur { get; protected set; }
		///<summary>We need access to a few other fields of the sheetDef.</summary>
		protected SheetDef _sheetDefCur;
		protected bool _isReadOnly;
		///<summary>When we open this form from the mobile layout window, make it clear which fields do not apply to mobile layout. False by default.</summary>
		protected bool _isEditMobile;
		protected List<SheetFieldDef> AvailFields;

        //Declare all the common UI elements here so that inheriting designers can display them
        protected System.Windows.Forms.Label labelXPos=new System.Windows.Forms.Label() { Text="X Pos" };
        protected System.Windows.Forms.Label labelYPos=new System.Windows.Forms.Label() { Text="Y Pos" };
        protected System.Windows.Forms.Label labelWidth=new System.Windows.Forms.Label() { Text="Width" };
        protected System.Windows.Forms.Label labelHeight=new System.Windows.Forms.Label() { Text="Height" };
        protected ValidNum textXPos=new ValidNum();
        protected ValidNum textYPos=new ValidNum();
        protected ValidNum textWidth=new ValidNum();
        protected ValidNum textHeight=new ValidNum();
        protected OpenDental.UI.Button butDelete=new UI.Button() { Text="&Delete" };
        protected OpenDental.UI.Button butOK=new UI.Button() { Text="&OK" };
        protected OpenDental.UI.Button butCancel=new UI.Button() { Text="&Cancel" };

        public FormSheetFieldBase() { }

		public FormSheetFieldBase(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly,bool isEditMobile=false) {
            SheetFieldDefCur=sheetFieldDef;
            _sheetDefCur=sheetDef;
			_isReadOnly=isReadOnly;
			_isEditMobile=isEditMobile;

            if(_isReadOnly) {
                butOK.Enabled=false;
                butDelete.Enabled=false;
            }
            if(_isEditMobile) {
                textXPos.Enabled=false;
                textYPos.Enabled=false;
                textWidth.Enabled=false;
                textHeight.Enabled=false;
            }
            textYPos.MaxVal=_sheetDefCur.HeightTotal-1;//The maximum y-value of the sheet field must be within the page vertically.
            textXPos.Text=SheetFieldDefCur.XPos.ToString();
            textYPos.Text=SheetFieldDefCur.YPos.ToString();
            textWidth.Text=SheetFieldDefCur.Width.ToString();
            textHeight.Text=SheetFieldDefCur.Height.ToString();
            this.butOK.Click+=new System.EventHandler(this.butOK_Click);
            this.butCancel.Click+=new System.EventHandler(this.butCancel_Click);
            this.butDelete.Click+=new System.EventHandler(this.butDelete_Click);
        }

        ///<summary>Validates the error provider on the common X and Y Position and Width/Height textboxes. If all are valid, also sets the 
        ///values for SheetFieldDefCur and returns true. Otherwise returns false without changing SheetFieldDefCur.</summary>
        protected bool ArePosAndSizeValid() {
            if(textXPos.errorProvider1.GetError(textXPos)!=""
                || textYPos.errorProvider1.GetError(textYPos)!=""
                || textWidth.errorProvider1.GetError(textWidth)!=""
                || textHeight.errorProvider1.GetError(textHeight)!="")
			{
                MsgBox.Show(this,"Please fix data entry errors first.");
                return false;
            }
            SheetFieldDefCur.XPos=PIn.Int(textXPos.Text);
            SheetFieldDefCur.YPos=PIn.Int(textYPos.Text);
            SheetFieldDefCur.Width=PIn.Int(textWidth.Text);
            SheetFieldDefCur.Height=PIn.Int(textHeight.Text);
            return true;
        }

        protected virtual void OnOk() { }

        protected virtual void OnDelete() {
            SheetFieldDefCur=null;
            DialogResult=DialogResult.OK;
        }

        protected virtual void OnCancel() {
            DialogResult=DialogResult.Cancel;
        }

        protected void butDelete_Click(object sender,EventArgs e) {
            OnDelete();
        }

        protected void butOK_Click(object sender,EventArgs e) {
            OnOk();
        }

        protected void butCancel_Click(object sender,EventArgs e) {
            OnCancel();
        }
    }
}
