using System;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormTaskListEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListBox listDateType;
		private System.Windows.Forms.TextBox textDescript;
		private OpenDental.ValidDate textDateTL;
		private TaskList Cur;
		private System.Windows.Forms.CheckBox checkFromNum;
		private System.Windows.Forms.ListBox listObjectType;
		private System.Windows.Forms.Label label6;
		private TextBox textTaskListNum;
		private Label labelTaskListNum;
		private UI.ComboBoxPlus comboGlobalFilter;
		private Label labelGlobalFilter;
		private ErrorProvider errorProvider1;

		///<summary></summary>
		public bool IsNew;

		///<summary></summary>
		public FormTaskListEdit(TaskList cur)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Cur=cur;
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaskListEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.labelGlobalFilter = new System.Windows.Forms.Label();
			this.comboGlobalFilter = new UI.ComboBoxPlus();
			this.textDescript = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.listDateType = new System.Windows.Forms.ListBox();
			this.checkFromNum = new System.Windows.Forms.CheckBox();
			this.textDateTL = new OpenDental.ValidDate();
			this.listObjectType = new System.Windows.Forms.ListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textTaskListNum = new System.Windows.Forms.TextBox();
			this.labelTaskListNum = new System.Windows.Forms.Label();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(395,223);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(395,182);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 4;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8,18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116,19);
			this.label1.TabIndex = 2;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelGlobalFilter
			// 
			this.labelGlobalFilter.Location = new System.Drawing.Point(8, 227);
			this.labelGlobalFilter.Name = "labelGlobalFilter";
			this.labelGlobalFilter.Size = new System.Drawing.Size(116, 19);
			this.labelGlobalFilter.TabIndex = 138;
			this.labelGlobalFilter.Text = "Global Filter Override";
			this.labelGlobalFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboGlobalFilter
			// 
			this.comboGlobalFilter.Location = new System.Drawing.Point(127, 227);
			this.comboGlobalFilter.Name = "comboGlobalFilter";
			this.comboGlobalFilter.Size = new System.Drawing.Size(120, 21);
			this.comboGlobalFilter.TabIndex = 137;
			this.comboGlobalFilter.SelectionChangeCommitted += new System.EventHandler(this.comboGlobalFilter_SelectionChangeCommitted);
			// 
			// textDescript
			// 
			this.textDescript.Location = new System.Drawing.Point(127,18);
			this.textDescript.Name = "textDescript";
			this.textDescript.Size = new System.Drawing.Size(293,20);
			this.textDescript.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8,50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(116,19);
			this.label2.TabIndex = 4;
			this.label2.Text = "Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(218,47);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(185,32);
			this.label3.TabIndex = 6;
			this.label3.Text = "Leave blank unless you want this list to show on a dated list";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8,82);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(116,19);
			this.label4.TabIndex = 7;
			this.label4.Text = "Date Type";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listDateType
			// 
			this.listDateType.Location = new System.Drawing.Point(127,83);
			this.listDateType.Name = "listDateType";
			this.listDateType.Size = new System.Drawing.Size(120,56);
			this.listDateType.TabIndex = 2;
			// 
			// checkFromNum
			// 
			this.checkFromNum.CheckAlign = System.Drawing.ContentAlignment.TopRight;
			this.checkFromNum.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkFromNum.Location = new System.Drawing.Point(8,149);
			this.checkFromNum.Name = "checkFromNum";
			this.checkFromNum.Size = new System.Drawing.Size(133,21);
			this.checkFromNum.TabIndex = 3;
			this.checkFromNum.Text = "Is From Repeating";
			this.checkFromNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateTL
			// 
			this.textDateTL.Location = new System.Drawing.Point(127,50);
			this.textDateTL.Name = "textDateTL";
			this.textDateTL.Size = new System.Drawing.Size(87,20);
			this.textDateTL.TabIndex = 1;
			// 
			// listObjectType
			// 
			this.listObjectType.Location = new System.Drawing.Point(127,173);
			this.listObjectType.Name = "listObjectType";
			this.listObjectType.Size = new System.Drawing.Size(120,43);
			this.listObjectType.TabIndex = 15;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8,172);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(116,19);
			this.label6.TabIndex = 16;
			this.label6.Text = "Object Type";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTaskListNum
			// 
			this.textTaskListNum.Location = new System.Drawing.Point(366,94);
			this.textTaskListNum.Name = "textTaskListNum";
			this.textTaskListNum.ReadOnly = true;
			this.textTaskListNum.Size = new System.Drawing.Size(54,20);
			this.textTaskListNum.TabIndex = 136;
			this.textTaskListNum.Visible = false;
			// 
			// labelTaskListNum
			// 
			this.labelTaskListNum.Location = new System.Drawing.Point(276,95);
			this.labelTaskListNum.Name = "labelTaskListNum";
			this.labelTaskListNum.Size = new System.Drawing.Size(88,16);
			this.labelTaskListNum.TabIndex = 135;
			this.labelTaskListNum.Text = "TaskListNum";
			this.labelTaskListNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelTaskListNum.Visible = false;
			// 
			// errorProvider1
			// 
			this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
			this.errorProvider1.ContainerControl = this;
			// 
			// FormTaskListEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(503, 274);
			this.Controls.Add(this.labelGlobalFilter);
			this.Controls.Add(this.comboGlobalFilter);
			this.Controls.Add(this.textTaskListNum);
			this.Controls.Add(this.labelTaskListNum);
			this.Controls.Add(this.listObjectType);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textDateTL);
			this.Controls.Add(this.textDescript);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.checkFromNum);
			this.Controls.Add(this.listDateType);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTaskListEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Task List";
			this.Load += new System.EventHandler(this.FormTaskListEdit_Load);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormTaskListEdit_Load(object sender, System.EventArgs e) {
			#if DEBUG
				labelTaskListNum.Visible=true;
				textTaskListNum.Visible=true;
				textTaskListNum.Text=Cur.TaskListNum.ToString();
			#endif
			textDescript.Text=Cur.Descript;
			if(Cur.DateTL.Year>1880){
				textDateTL.Text=Cur.DateTL.ToShortDateString();
			}
			for(int i=0;i<Enum.GetNames(typeof(TaskDateType)).Length;i++){
				listDateType.Items.Add(Lan.g("enumTaskDateType",Enum.GetNames(typeof(TaskDateType))[i]));
				if((int)Cur.DateType==i){
					listDateType.SelectedIndex=i;
				}
			}
			if(Cur.FromNum==0){
				checkFromNum.Checked=false;
				checkFromNum.Enabled=false;
			}
			else{
				checkFromNum.Checked=true;
			}
			if(Cur.IsRepeating){
				textDateTL.Enabled=false;
				listObjectType.Enabled=false;
				if(Cur.Parent!=0){//not a main parent
					listDateType.Enabled=false;
				}
			}
			for(int i=0;i<Enum.GetNames(typeof(TaskObjectType)).Length;i++){
				listObjectType.Items.Add(Lan.g("enumTaskObjectType",Enum.GetNames(typeof(TaskObjectType))[i]));
				if((int)Cur.ObjectType==i){
					listObjectType.SelectedIndex=i;
				}
			}
			FillComboGlobalFilter();
		}

		private void FillComboGlobalFilter() {
			if((GlobalTaskFilterType)PrefC.GetInt(PrefName.TasksGlobalFilterType)==GlobalTaskFilterType.Disabled) {
				comboGlobalFilter.Visible=false;
				labelGlobalFilter.Visible=false;
				return;
			}
			comboGlobalFilter.Items.Add(Lan.g(this,GlobalTaskFilterType.Default.GetDescription()),GlobalTaskFilterType.Default);
			comboGlobalFilter.Items.Add(Lan.g(this,GlobalTaskFilterType.None.GetDescription()),GlobalTaskFilterType.None);
			if(PrefC.HasClinicsEnabled) {
				comboGlobalFilter.Items.Add(Lan.g(this,GlobalTaskFilterType.Clinic.GetDescription()),GlobalTaskFilterType.Clinic);
				if(Defs.GetDefsForCategory(DefCat.Regions).Count>0) {
					comboGlobalFilter.Items.Add(Lan.g(this,GlobalTaskFilterType.Region.GetDescription()),GlobalTaskFilterType.Region);
				}
			}
			comboGlobalFilter.SetSelectedEnum(Cur.GlobalTaskFilterType);
			if(comboGlobalFilter.SelectedIndex==-1) {
				errorProvider1.SetError(comboGlobalFilter,$"Previous selection \"{Cur.GlobalTaskFilterType.GetDescription()}\" is no longer available.  "
					+"Saving will overwrite previous setting.");
				comboGlobalFilter.SelectedIndex=0;
			}
		}

		private void comboGlobalFilter_SelectionChangeCommitted(object sender,EventArgs e) {
			errorProvider1.SetError(comboGlobalFilter,string.Empty);//Clear the error, if applicable.
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(  textDateTL.errorProvider1.GetError(textDateTL)!=""
				){
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			Cur.Descript=textDescript.Text;
			Cur.DateTL=PIn.Date(textDateTL.Text);
			Cur.DateType=(TaskDateType)listDateType.SelectedIndex;
			if(!checkFromNum.Checked){//user unchecked the box
				Cur.FromNum=0;
			}
			Cur.ObjectType=(TaskObjectType)listObjectType.SelectedIndex;
			Cur.GlobalTaskFilterType=comboGlobalFilter.GetSelected<GlobalTaskFilterType>();
			try{
				if(IsNew) {
					TaskLists.Insert(Cur);
					SecurityLogs.MakeLogEntry(Permissions.TaskListCreate,0,Cur.Descript+" "+Lan.g(this,"added"));
				}
				else {
					TaskLists.Update(Cur);
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		


	}
}





















