using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	///<summary></summary>
	public delegate void ButtonClickedEventHandler(object sender,ButtonClicked_EventArgs e);
	/// <summary></summary>
	public class OutlookBar : System.Windows.Forms.Control{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public OutlookButton[] Buttons;
		private ImageList imageList32;
		///<summary></summary>
		public int SelectedIndex=-1;
		private int currentHot=-1;
		private Font textFont=new Font("Arial",8);
		///<summary></summary>
		[Category("Action"),Description("Occurs when a button is clicked.")]
		public event ButtonClickedEventHandler ButtonClicked = null;
		///<summary>Used when click event is cancelled.</summary>
		private int previousSelected;
		///<summary>Class level variable, to avoid allocating and disposing memory repeatedly every frame.</summary>
		private StringFormat _format;

		///<summary></summary>
		public OutlookBar(){
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.DoubleBuffered=true; //reduces flicker
			SetTheme();
			Buttons=new OutlookButton[7];
			Buttons[0]=new OutlookButton("Appts",imageList32.Images[ODColorTheme.OutlookApptImageIndex]);
			Buttons[1]=new OutlookButton("Family",imageList32.Images[ODColorTheme.OutlookFamilyImageIndex]);
			Buttons[2]=new OutlookButton("Account",imageList32.Images[ODColorTheme.OutlookAcctImageIndex]);
			Buttons[3]=new OutlookButton("Treat' Plan",imageList32.Images[ODColorTheme.OutlookTreatPlanImageIndex]);
			Buttons[4]=new OutlookButton("Chart",imageList32.Images[ODColorTheme.OutlookChartImageIndex]);
			Buttons[5]=new OutlookButton("Images",imageList32.Images[ODColorTheme.OutlookImagesImageIndex]);
			Buttons[6]=new OutlookButton("Manage",imageList32.Images[ODColorTheme.OutlookManageImageIndex]);
			UpdateAll();
		}

		///<summary>Hard coded theme for now.</summary>
		private void SetTheme() {
			_format=new StringFormat();
			_format.Alignment=StringAlignment.Center;
		}

		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutlookBar));
			this.imageList32 = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageList32
			// 
			this.imageList32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList32.ImageStream")));
			this.imageList32.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList32.Images.SetKeyName(0, "Appt32.gif");
			this.imageList32.Images.SetKeyName(1, "Family32b.gif");
			this.imageList32.Images.SetKeyName(2, "Account32b.gif");
			this.imageList32.Images.SetKeyName(3, "TreatPlan3D.gif");
			this.imageList32.Images.SetKeyName(4, "chart32.gif");
			this.imageList32.Images.SetKeyName(5, "Images32.gif");
			this.imageList32.Images.SetKeyName(6, "Manage32.gif");
			this.imageList32.Images.SetKeyName(7, "TreatPlanMed32.gif");
			this.imageList32.Images.SetKeyName(8, "ChartMed32.gif");
			this.imageList32.Images.SetKeyName(9, "Date-32_Blue.png");
			this.imageList32.Images.SetKeyName(10, "User-Group-32_Blue.png");
			this.imageList32.Images.SetKeyName(11, "Money-Credit-Card-32_Blue.png");
			this.imageList32.Images.SetKeyName(12, "Payments-32_Blue.png");
			this.imageList32.Images.SetKeyName(13, "Dentist-32_Blue.png");
			this.imageList32.Images.SetKeyName(14, "Folder-Picture-32_Blue.png");
			this.imageList32.Images.SetKeyName(15, "Gear-32_Blue.png");
			this.ResumeLayout(false);

		}
		#endregion

		public void RefreshButtons() {
			Buttons[0].Caption=Lan.g(this,"Appts");
			Buttons[0].Image=imageList32.Images[ODColorTheme.OutlookApptImageIndex];
			Buttons[1].Caption=Lan.g(this,"Family");
			Buttons[1].Image=imageList32.Images[ODColorTheme.OutlookFamilyImageIndex];
			Buttons[2].Caption=Lan.g(this,"Account");
			Buttons[2].Image=imageList32.Images[ODColorTheme.OutlookAcctImageIndex];
			Buttons[3].Caption=Lan.g(this,"Treat' Plan");
			Buttons[3].Image=imageList32.Images[ODColorTheme.OutlookTreatPlanImageIndex];
			Buttons[4].Caption=Lan.g(this,"Chart");
			Buttons[4].Image=imageList32.Images[ODColorTheme.OutlookChartImageIndex];
			Buttons[5].Caption=Lan.g(this,"Images");
			Buttons[5].Image=imageList32.Images[ODColorTheme.OutlookImagesImageIndex];
			Buttons[6].Caption=Lan.g(this,"Manage");
			Buttons[6].Image=imageList32.Images[ODColorTheme.OutlookManageImageIndex];
			if(PrefC.GetBool(PrefName.EasyHideClinical)) {
				Buttons[4].Caption=Lan.g(this,"Procs");
			}
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				Buttons[3].Image=imageList32.Images[ODColorTheme.OutlookEcwTreatPlanImageIndex];
				Buttons[4].Image=imageList32.Images[ODColorTheme.OutlookEcwChartImageIndex];
			}
		}
		
		/// <summary>Triggered every time the control decides to repaint itself.</summary>
		/// <param name="pe"></param>
		protected override void OnPaint(PaintEventArgs pe) {
			try {
				CalculateButtonInfo();
				bool isHot;
				bool isSelected;
				bool isPressed;
				pe.Graphics.DrawLine(Pens.Gray,Width-1,0,Width-1,Height-1);
				for(int i=0;i<Buttons.Length;i++) {
					Point mouseLoc=PointToClient(MousePosition);
					isHot=Buttons[i].Bounds.Contains(mouseLoc);
					isPressed=(MouseButtons==MouseButtons.Left && isHot);
					isSelected=(i==SelectedIndex);
					DrawButton(Buttons[i],isHot,isPressed,isSelected,pe.Graphics);
				}
			}
			catch(Exception ex) {
				//We had one customer who was receiving overflow exceptions because the ClientRetangle provided by the system was invalid,
				//due to a graphics device hardware state change when loading the Dexis client application via our Dexis bridge.
				//If we receive an invalid ClientRectangle, then we will simply not draw the button for a frame or two until the system has initialized.
				//A couple of frames later the system should return to normal operation and we will be able to draw the button again.
				ex.DoNothing();
			}
			// Calling the base class OnPaint
			base.OnPaint(pe);
		}

		/// <summary>Draws one button using the info specified.</summary>
		/// <param name="myButton">Contains caption, image and bounds info.</param>
		/// <param name="isHot">Is the mouse currently hovering over this button.</param>
		/// <param name="isPressed">Is the left mouse button currently down on this button.</param>
		/// <param name="isSelected">Is this the currently selected button</param>
		private void DrawButton(OutlookButton myButton,bool isHot,bool isPressed,bool isSelected,Graphics g){
			if(!myButton.Visible) {
				g.FillRectangle(ODColorTheme.OutlookBackBrush,myButton.Bounds.X,myButton.Bounds.Y
					,myButton.Bounds.Width+1,myButton.Bounds.Height+1);
				return;
			}
			if(isPressed) {
				g.FillRectangle(ODColorTheme.OutlookPressedBrush,myButton.Bounds.X,myButton.Bounds.Y,myButton.Bounds.Width+1,myButton.Bounds.Height+1);
			}
			else if(isSelected) {
				g.FillRectangle(ODColorTheme.OutlookSelectedBrush,myButton.Bounds.X,myButton.Bounds.Y,myButton.Bounds.Width+1,myButton.Bounds.Height+1);
				Rectangle gradientRect=new Rectangle(myButton.Bounds.X,myButton.Bounds.Y+myButton.Bounds.Height-10,myButton.Bounds.Width,10);
				//Recreate _hotBrush if theme color changed.
				if(!myButton.Color1HotBrush.Equals(ODColorTheme.OutlookSelectedBrush.Color) || 
					!myButton.Color2HotBrush.Equals(ODColorTheme.OutlookPressedBrush.Color))
				{
					if(myButton.HotBrush!=null) {
						myButton.HotBrush.Dispose();
					}
					myButton.Color1HotBrush=ODColorTheme.OutlookSelectedBrush.Color;
					myButton.Color2HotBrush=ODColorTheme.OutlookPressedBrush.Color;
					myButton.HotBrush=new LinearGradientBrush(gradientRect,myButton.Color1HotBrush,myButton.Color2HotBrush,LinearGradientMode.Vertical);
				}
				g.FillRectangle(myButton.HotBrush,myButton.Bounds.X,myButton.Bounds.Y+myButton.Bounds.Height-10,myButton.Bounds.Width+1,10);
			}
			else if(isHot) {
				g.FillRectangle(ODColorTheme.OutlookHotBrush,myButton.Bounds.X,myButton.Bounds.Y,myButton.Bounds.Width+1,myButton.Bounds.Height+1);
			}
			else {
				g.FillRectangle(ODColorTheme.OutlookBackBrush,myButton.Bounds.X,myButton.Bounds.Y,myButton.Bounds.Width+1,myButton.Bounds.Height+1);
			}
			//outline
			if(isPressed || isSelected || isHot) {
				//block out the corners so they won't show.  This can be improved later.
				g.FillPolygon(ODColorTheme.OutlookBackBrush,new Point[] {
				new Point(myButton.Bounds.X,myButton.Bounds.Y),
				new Point(myButton.Bounds.X+3,myButton.Bounds.Y),
				new Point(myButton.Bounds.X,myButton.Bounds.Y+3)});
				g.FillPolygon(ODColorTheme.OutlookBackBrush,new Point[] {//it's one pixel to the right because of the way rect drawn.
				new Point(myButton.Bounds.X+myButton.Bounds.Width-2,myButton.Bounds.Y),
				new Point(myButton.Bounds.X+myButton.Bounds.Width+1,myButton.Bounds.Y),
				new Point(myButton.Bounds.X+myButton.Bounds.Width+1,myButton.Bounds.Y+3)});
				g.FillPolygon(ODColorTheme.OutlookBackBrush,new Point[] {//it's one pixel down and right.
				new Point(myButton.Bounds.X+myButton.Bounds.Width+1,myButton.Bounds.Y+myButton.Bounds.Height-3),
				new Point(myButton.Bounds.X+myButton.Bounds.Width+1,myButton.Bounds.Y+myButton.Bounds.Height+1),
				new Point(myButton.Bounds.X+myButton.Bounds.Width-3,myButton.Bounds.Y+myButton.Bounds.Height+1)});
				g.FillPolygon(ODColorTheme.OutlookBackBrush,new Point[] {//it's one pixel down
				new Point(myButton.Bounds.X,myButton.Bounds.Y+myButton.Bounds.Height-3),
				new Point(myButton.Bounds.X+3,myButton.Bounds.Y+myButton.Bounds.Height+1),
				new Point(myButton.Bounds.X,myButton.Bounds.Y+myButton.Bounds.Height+1)});
				//then draw outline
				GraphicsHelper.DrawRoundedRectangle(g,ODColorTheme.OutlookOutlineColor,myButton.Bounds,ODColorTheme.OutlookHoverCornerRadius);
			}
			//Image
			Rectangle imgRect=new Rectangle((Width-myButton.Image.Width)/2,myButton.Bounds.Y+3,myButton.Image.Width,myButton.Image.Height);
			if(myButton.Image!=null) {
				ODException.SwallowAnyException(() => {
					//If fails, then would be a bad reason to crash.  Should still draw button text below so user knows what the button is.
					//Consider using ODColorTheme.InvertImageIfNeeded(myButton.Image) in future for dark themes,
					//however, if we use the inverted image, then we need to prevent processing the inverted image every frame.
					g.DrawImage(myButton.Image,imgRect);
				});
			}
			//Text
			Rectangle textRect = new Rectangle(myButton.Bounds.X-1,imgRect.Bottom+3,myButton.Bounds.Width+2,myButton.Bounds.Bottom-imgRect.Bottom+3);
			g.DrawString(myButton.Caption,textFont,ODColorTheme.OutlookTextBrush,textRect,_format);
		}

		internal void UpdateAll(){
			// Calculates Button info and redraws all.
			//if(!m_BeginUpdate){
				CalculateButtonInfo();
				this.Invalidate();
			//}
		}

		private void CalculateButtonInfo(){
			// Calculates button sizes and maybe more later
			//int barTop = 1;
			using(Graphics g = this.CreateGraphics()){
				int top=0;
				int width=this.Width-2;
				int textHeight=0;
				for(int i=0;i<Buttons.Length;i++){
					//--- Look if multiline text, if is add extra Height to button.
					SizeF textSize = g.MeasureString(Buttons[i].Caption,textFont,width+2);
					textHeight = (int)(Math.Ceiling(textSize.Height));
					if(textHeight<26)
						textHeight=26;//default to height of 2 lines of text for uniformity.
					Buttons[i].Bounds=new Rectangle(0,top,width,39+textHeight);
					top+=39+textHeight+1;
				}//for
			}//using

		}

		/*private ImageAttributes GetInverseAttributes() {
			ColorMatrix c = new ColorMatrix(new float[][] {
				new float[] {-1, 0, 0, 0, 0},
				new float[] {0, -1, 0, 0, 0},
				new float[] {0, 0, -1, 0, 0},
				new float[] {0, 0, 0, 1, 0},
				new float[] {1, 1, 1, 0, 1}
			});
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(c);
			return attributes;
		}*/

		///<summary></summary>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e){
			base.OnMouseMove(e);
			int hotBut=GetButtonI(new Point(e.X,e.Y));
			if(hotBut != currentHot){
				Invalidate(); //just invalidate to force a repaint.
				currentHot=hotBut;
			}			
		}

		///<summary></summary>
		protected override void OnMouseLeave(System.EventArgs e){
			base.OnMouseLeave(e);
			if(currentHot!=-1){
				//undraw previous button
				Invalidate(); //just invalidate to force a repaint.
			}
			currentHot=-1;		
		}

		///<summary></summary>
		protected override void OnSizeChanged(System.EventArgs e){
			base.OnSizeChanged(e);
			this.UpdateAll();
		}

		private int GetButtonI(Point myPoint){
			for(int i=0;i<Buttons.Length;i++){
				//Item item = activeBar.Items[it];
				if(Buttons[i].Bounds.Contains(myPoint)){
					return i;
				}
			}//for
			return -1;
		}

		///<summary></summary>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e){
			base.OnMouseDown(e);
			//Graphics g=this.CreateGraphics();
			if(currentHot != -1){
				//redraw current button to give feedback on mouse down.
				Invalidate(); //just invalidate to force a repaint
			}
		}

		///<summary></summary>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e){
			base.OnMouseUp(e);
			if(e.Button != MouseButtons.Left){
				return;
			}
			int selectedBut=GetButtonI(new Point(e.X,e.Y));
			if(selectedBut==-1){
				return;
			}
			if(!Buttons[selectedBut].Visible){
				return;
			}
			int oldSelected=SelectedIndex;
			this.previousSelected=SelectedIndex;
			SelectedIndex=selectedBut;
			Invalidate(); //just invalidate to force a repaint
			OnButtonClicked(Buttons[SelectedIndex],false);
		}

		///<summary></summary>
		protected void OnButtonClicked(OutlookButton myButton,bool myCancel){
			if(this.ButtonClicked != null){
				//previousSelected=SelectedIndex;
				ButtonClicked_EventArgs oArgs = new ButtonClicked_EventArgs(myButton,myCancel);
				this.ButtonClicked(this,oArgs);
				if(oArgs.Cancel){
					SelectedIndex=previousSelected;
					Invalidate();
				}
			}
		}


	}

	///<summary></summary>
	public struct OutlookButton{//this should be a class if I was going to ever reuse this control
		//private string caption;
		//private int imageIndex;
		///<summary></summary>
		public OutlookButton(string caption,Image image){
			Caption=caption;
			Image=image;
			Bounds=new Rectangle(0,0,0,0);
			Visible=true;
			HotBrush=null;
			Color1HotBrush=Color.Transparent;
			Color2HotBrush=Color.Transparent;
		}

		///<summary></summary>
		public string Caption;
		///<summary></summary>
		public Image Image;
		///<summary></summary>
		public Rectangle Bounds;
		///<summary></summary>
		public bool Visible;
		///<summary>Used inside OutlookBar logic to track theme colors and location.  Do not modify externally!</summary>
		public LinearGradientBrush HotBrush;
		///<summary>Used inside OutlookBar logic to track theme color.  Do not modify externally!</summary>
		public Color Color1HotBrush;
		///<summary>Used inside OutlookBar logic to track theme color.  Do not modify externally!</summary>
		public Color Color2HotBrush;

	}

	///<summary></summary>
	public class ButtonClicked_EventArgs{
		private OutlookButton outlookButton;
		private bool cancel;

		///<summary></summary>
		public ButtonClicked_EventArgs(OutlookButton myButton,bool myCancel){
			outlookButton=myButton;
		}

		///<summary></summary>
		public OutlookButton OutlookButton{
			get{
				return outlookButton;
			}
		}

		///<summary>Set true to cancel the event.</summary>
		public bool Cancel{
			get{
				return cancel;
			}
			set{
				cancel=value;
			}
		}

	}

}







