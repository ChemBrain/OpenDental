using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;

namespace OpenDental {
	///<summary>This form takes the image the user wants to send through DentalXChange and gathers the additional information (specified by the user)
	///that is needed to create the attachment through ClaimConnect's API.</summary>
	public partial class FormClaimAttachmentItemEdit:ODForm {
		public ClaimConnect.ImageAttachment ImageAttachment=new ClaimConnect.ImageAttachment();
		private Image _claimImage;
		private int _startingHeight;

		///<summary>Used for opening and editing an existing image attachment row in FormClaimAttachment.</summary>
		public FormClaimAttachmentItemEdit(Image image,string fileName,DateTime date,ClaimConnect.ImageTypeCode imageType) : this(image){
			textFileName.Text=fileName;
			textDateCreated.Text=date.ToShortDateString();//Override today's date with the passed in date.
			comboImageType.SelectedIndex=(int)imageType;
		}

		///<summary>Takes an image the user has chosen to send with their claim.</summary>
		public FormClaimAttachmentItemEdit(Image image) {
			InitializeComponent();
			Lan.F(this);
			comboImageType.Items.Clear();
			foreach(ClaimConnect.ImageTypeCode imageTypeCode in Enum.GetValues(typeof(ClaimConnect.ImageTypeCode))) {
				comboImageType.Items.Add(imageTypeCode.GetDescription(),imageTypeCode);
			}
			_claimImage=image;
			textDateCreated.Text=DateTime.Today.ToShortDateString();
			_startingHeight=this.Height;
			RelayoutForm();
		}

		private void RelayoutForm() {
			if(comboImageType.GetSelected<ClaimConnect.ImageTypeCode>()==ClaimConnect.ImageTypeCode.XRays) {
				this.Size=new Size(this.Width,_startingHeight);
				groupBoxOrientationType.Visible=true;
			}
			else {
				this.Size=new Size(this.Width,_startingHeight-groupBoxOrientationType.Height);
				groupBoxOrientationType.Visible=false;
			}
		}

		private void ComboImageType_SelectionChangeCommitted_1(object sender, EventArgs e){
			RelayoutForm();
		}

		///<summary>Called on OK_Click(). This method takes the user entered data and creates the ClaimConnect.ImageAttachment object
		///used by FormClaimAttach. This object is eventually used in the ClaimConnect.CreateAttachment() API call to DentalXChange.</summary>
		private void CreateImageAttachment() {
			ImageAttachment.ImageFileNameDisplay=textFileName.Text;
			ImageAttachment.ImageDate=PIn.Date(textDateCreated.Text);
			ImageAttachment.ImageType=comboImageType.GetSelected<ClaimConnect.ImageTypeCode>();
			if(radioButtonLeft.Checked) {
				ImageAttachment.ImageOrientationType="left";
			}
			else {
				ImageAttachment.ImageOrientationType="right";
			}
			ImageAttachment.ImageFileAsBase64=ConvertImageToBytes(_claimImage);
			ImageAttachment.Image=_claimImage;
		}

		///<summary>Takes the user's image they want to send with their claim and converts it to a base64 byte representation.
		///ClaimConnect requires the image to be in this format.</summary>
		private byte[] ConvertImageToBytes(Image image) {
			using(MemoryStream m=new MemoryStream()) {
				image.Save(m,ImageFormat.Jpeg);
				return m.ToArray();
			}
		}

		///<summary></summary>
		private void buttonOK_Click(object sender,EventArgs e) {
			if(string.IsNullOrWhiteSpace(textFileName.Text)) {
				MsgBox.Show(this,"Enter the filename for this attachment.");
				return;
			}
			if(textFileName.Text.IndexOfAny(Path.GetInvalidFileNameChars())>=0) {//returns -1 if nothing found
				MsgBox.Show(this,"Invalid characters detected in the filename. Please remove them and try again.");
				return;
			}
			if(comboImageType.SelectedIndex==-1) {
				MsgBox.Show(this,"Choose an image type.");
				return;
			}
			if(textDateCreated.errorProvider1.GetError(textDateCreated)!="") {
				MsgBox.Show(this,"Enter a valid date.");
				return;
			}
			CreateImageAttachment();
			DialogResult=DialogResult.OK;
		}

		private void buttonCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
	}
}