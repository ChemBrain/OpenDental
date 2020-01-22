using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSheetFieldStatic:FormSheetFieldBase {
		private int textSelectionStart;

		public FormSheetFieldStatic(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly,bool isEditMobile=false): base(sheetDef,sheetFieldDef,isReadOnly,isEditMobile) {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetFieldStatic_Load(object sender,EventArgs e) {
			if(_sheetDefCur.SheetType!=SheetTypeEnum.Statement) {
				checkPmtOpt.Visible=false;
			}
			if(_sheetDefCur.SheetType==SheetTypeEnum.PatientLetter) {
				butExamSheet.Visible=true;
			}
			else {
				butExamSheet.Visible=false;
			}
			if(_isEditMobile) { //When we open this form from the mobile layout window, make it clear which fields do not apply to mobile layout
				groupBox1.Enabled=false;
				comboGrowthBehavior.Enabled=false;
				checkPmtOpt.Enabled=false;
			}
			if(SheetDefs.IsDashboardType(_sheetDefCur)) {
				comboGrowthBehavior.Enabled=false;
			}
			checkIncludeInMobile.Visible=SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType);
			//Show/hide in mobile editor depending on if TabOrderMobile has been previously set. This is how we will selectively include only desireable StaticText fields.
			checkIncludeInMobile.Checked=SheetFieldDefCur.TabOrderMobile>=1;
			checkIsLocked.Checked=SheetFieldDefCur.IsNew ? true : SheetFieldDefCur.IsLocked;
			textFieldValue.Text=SheetFieldDefCur.FieldValue;
			InstalledFontCollection fColl=new InstalledFontCollection();
			for(int i=0;i<fColl.Families.Length;i++){
				comboFontName.Items.Add(fColl.Families[i].Name);
			}
			comboFontName.Text=SheetFieldDefCur.FontName;
			numFontSize.Value=(decimal)SheetFieldDefCur.FontSize;
			checkFontIsBold.Checked=SheetFieldDefCur.FontIsBold;
			SheetUtil.FillComboGrowthBehavior(comboGrowthBehavior,SheetFieldDefCur.GrowthBehavior);
			for(int i=0;i<Enum.GetNames(typeof(System.Windows.Forms.HorizontalAlignment)).Length;i++) {
				comboTextAlign.Items.Add(Enum.GetNames(typeof(System.Windows.Forms.HorizontalAlignment))[i]);
				if((int)SheetFieldDefCur.TextAlign==i) {
					comboTextAlign.SelectedIndex=i;
				}
			}
			checkPmtOpt.Checked=SheetFieldDefCur.IsPaymentOption;
			butColor.BackColor=SheetFieldDefCur.ItemColor;
			FillFields();
		}

		private void FillFields(){
			string[] fieldArray=Enum.GetValues(typeof(StaticTextField)).Cast<StaticTextField>().Where(x => !x.IsStaticTextFieldObsolete())
				//Not including patientPortalCredentials because simply viewing the dashboard would create a username and password for the patient
				.Where(x => !(SheetDefs.IsDashboardType(_sheetDefCur) && x==StaticTextField.patientPortalCredentials))
				.Select(x => x.GetDescription()).ToArray();
			listFields.Items.Clear();
			for(int i=0;i<fieldArray.Length;i++){
				listFields.Items.Add(fieldArray[i]);
			}
		}

		private void ListFields_MouseDown(object sender,MouseEventArgs e) {
			if(e.Button!=MouseButtons.Left) {
				return;
			}
			//SelectedItem will be null if the user clicks inside the ListBox but not on an item in the list.
			if(sender.GetType()!=typeof(ListBox) || ((ListBox)sender).SelectedItem==null) {
				return;
			}
			string fieldStr=((ListBox)sender).SelectedItem.ToString();
			if(textSelectionStart < textFieldValue.Text.Length-1) {
				textFieldValue.Text=textFieldValue.Text.Substring(0,textSelectionStart)
					+"["+fieldStr+"]"
					+textFieldValue.Text.Substring(textSelectionStart);
			}
			else{//otherwise, just tack it on the end
				textFieldValue.Text+="["+fieldStr+"]";
			}
			textFieldValue.Select(textSelectionStart+fieldStr.Length+2,0);
			textFieldValue.Focus();
			listFields.ClearSelected();
			//if(!textFieldValue.Focused){
			//	textFieldValue.Text+="["+fieldStr+"]";
			//	return;
			//}
			//MessageBox.Show(textFieldValue.SelectionStart.ToString());
		}

		private void textFieldValue_Leave(object sender,EventArgs e) {
			textSelectionStart=textFieldValue.SelectionStart;
		}

		/// <summary>This method is tied to any event that could change text size, such as font size, text, or the Bold checkbox.</summary>
		private void UpdateTextSizeLabels(object sender,EventArgs e) {
			float fontSize=(float)numFontSize.Value;
			FontStyle curFontStyle=FontStyle.Regular;
			if(checkFontIsBold.Checked) {
				curFontStyle=FontStyle.Bold;
			}
			Size sizeText;
			using(Font font=new Font(comboFontName.Text,fontSize,curFontStyle)){
				//If we measure using a graphics object, it will report the size of the text if we drew it with a graphics object.
				//This correctly reports the text size for how we are drawing the text.
				sizeText=TextRenderer.MeasureText(textFieldValue.Text,font);
			}
			labelTextW.Text=Lan.g(this,"TextW:")+" "+sizeText.Width.ToString();
			labelTextH.Text=Lan.g(this,"TextH:")+" "+sizeText.Height.ToString();
		}

		private void butExamSheet_Click(object sender,EventArgs e) {
			FormSheetFieldExam FormE=new FormSheetFieldExam();
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(textSelectionStart < textFieldValue.Text.Length-1) {//if cursor is not at the end of the text in textFieldValue, insert into text beginning at cursor
				textFieldValue.Text=textFieldValue.Text.Substring(0,textSelectionStart)
				+"["+FormE.ExamFieldSelected+"]"
				+textFieldValue.Text.Substring(textSelectionStart);
			}
			else {//otherwise, just tack it on the end
				textFieldValue.Text+="["+FormE.ExamFieldSelected+"]";
			}
			textFieldValue.Select(textSelectionStart+FormE.ExamFieldSelected.Length+2,0);
			textFieldValue.Focus();
		}

		private void butColor_Click(object sender,EventArgs e) {
			ColorDialog colorDialog1=new ColorDialog();
			colorDialog1.Color=butColor.BackColor;
			colorDialog1.ShowDialog();
			butColor.BackColor=colorDialog1.Color;
		}

		protected override void OnOk() {
			if(!ArePosAndSizeValid()) {
				return;
			}
			if(textFieldValue.Text==""){
				MsgBox.Show(this,"Please set a field value first.");
				return;
			}
			if(comboFontName.Text==""){
				//not going to bother testing for validity unless it will cause a crash.
				MsgBox.Show(this,"Please select a font name first.");
				return;
			}
			float fontSize=(float)numFontSize.Value;
			if(fontSize<2){
				MsgBox.Show(this,"Font size is invalid.");
				return;
			}
			if(SheetDefs.IsDashboardType(_sheetDefCur) 
				&& textFieldValue.Text.ToLower().Contains($"[{StaticTextField.patientPortalCredentials.ToString().ToLower()}]")) 
			{
				MsgBox.Show(this,"The [patientPortalCredentials] tag is not allowed in Dashboards.");
				return;
			}
			SheetFieldDefCur.FieldValue=textFieldValue.Text;
			SheetFieldDefCur.FontName=comboFontName.Text;
			SheetFieldDefCur.FontSize=fontSize;
			SheetFieldDefCur.FontIsBold=checkFontIsBold.Checked;
			SheetFieldDefCur.GrowthBehavior=comboGrowthBehavior.GetSelected<GrowthBehaviorEnum>();
			SheetFieldDefCur.TextAlign=(System.Windows.Forms.HorizontalAlignment)comboTextAlign.SelectedIndex;
			SheetFieldDefCur.IsPaymentOption=checkPmtOpt.Checked;
			SheetFieldDefCur.ItemColor=butColor.BackColor;
			SheetFieldDefCur.IsLocked=checkIsLocked.Checked;
			if(checkIncludeInMobile.Checked && SheetDefs.IsMobileAllowed(_sheetDefCur.SheetType)) { //Had previously been hidden from mobile layout so show and set to top. User can re-order using the mobile editor.
				SheetFieldDefCur.TabOrderMobile=1;
			}
			else {
				SheetFieldDefCur.TabOrderMobile=0;
			}
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}
	}
}