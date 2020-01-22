using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;

namespace OpenDental {
	///<summary></summary>
	public partial class FormClaimAttachment:Form {
		private Claim _claimCur;
		private Patient _claimPat;
		private List<ClaimConnect.ImageAttachment> _listImageAttachments;
		private static ODThread _threadFormClaimAttach=null;

		///<summary>Initialize the form and refresh the claim we are adding attachments to.</summary>
		private FormClaimAttachment(Claim claim) {
			InitializeComponent();
			_claimCur=claim;
			_listImageAttachments=new List<ClaimConnect.ImageAttachment>();
		}

		private void FormClaimAttachment_Load(object sender,EventArgs e) {
			_claimPat=Patients.GetPat(_claimCur.PatNum);
			List<Def> imageTypeCategories=new List<Def>();
			List<Def> listClaimAttachmentDefs=CheckImageCatDefs();
			if(listClaimAttachmentDefs.Count<1) {//At least one Claim Attachment image definition exists.
				labelClaimAttachWarning.Visible=true;
			}
			FillGrid();
			ODProgress.ShowAction(()=> {ValidateClaimHelper();},"Communicating with DentalXChange...");
		}

		private void FormClaimAttachment_Shown(object sender,EventArgs e) {
			//Check for if the attachmentID is already in use. If so inform the user they need to redo their attachments.
			if(textClaimStatus.Text.ToUpper().Contains("ATTACHMENT ID HAS BEEN ASSOCIATED TO A DIFFERENT CLAIM")
				|| textClaimStatus.Text.ToUpper().Contains("HAS ALREADY BEEN DELIVERED TO THE PAYER"))
			{
				MessageBox.Show("The attachment ID is associated to another claim. Please redo your attachments.");
				_claimCur.AttachmentID="";
				ODProgress.ShowAction(()=> { ValidateClaimHelper(); },"Re-validating the claim...");
			}
		}

		///<summary></summary>
		public static void Open(Claim claim,Action actionOkClick=null) {
			//Show a dialog if the user tries to open more than one claim at a time
			if(_threadFormClaimAttach!=null) {
				MsgBox.Show("A claim attachment window is already open.");
				return;
			}
			_threadFormClaimAttach=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				FormClaimAttachment formCA=new FormClaimAttachment(claim);
				if(formCA.ShowDialog()==DialogResult.OK) {
					actionOkClick?.Invoke();
				}
			}));
			//This thread needs to be STA because FormClaimAttachment will have the ability to open child forms,
			//which will need to block FormClaimAttachment while it is open.
			//STA mode is the only way to have ShowDialog() behavior in a non-main thread,
			//without throwing an exception (the exception literally says change the thread to STA)
			_threadFormClaimAttach.SetApartmentState(System.Threading.ApartmentState.STA);
			_threadFormClaimAttach.AddExitHandler(new ODThread.WorkerDelegate((ODThread o) => {
				_threadFormClaimAttach=null;
			}));
			_threadFormClaimAttach.Start(true);
		}

		///<summary>Purposely does not load in historical data. This form is only for creating new attachments.
		///The grid is populated by AddImageToGrid().</summary>
		private void FillGrid() {
			gridAttachedImages.BeginUpdate();
			gridAttachedImages.ListGridColumns.Clear();
			gridAttachedImages.ListGridRows.Clear();
			GridColumn col;
			col=new GridColumn("Date", 80);
			gridAttachedImages.ListGridColumns.Add(col);
			col=new GridColumn("Image Type",150);
			gridAttachedImages.ListGridColumns.Add(col);
			col=new GridColumn("File",150);
			gridAttachedImages.ListGridColumns.Add(col);
			GridRow row;
			for(int i=0;i<_listImageAttachments.Count;i++) {
				row=new GridRow();
				row.Cells.Add(_listImageAttachments[i].ImageDate.ToShortDateString());
				row.Cells.Add(_listImageAttachments[i].ImageType.GetDescription());
				row.Cells.Add(_listImageAttachments[i].ImageFileNameDisplay);
				row.Tag=_listImageAttachments[i];
				gridAttachedImages.ListGridRows.Add(row);
			}
			gridAttachedImages.EndUpdate();
		}

		///<summary>Checks to see if the user has made a Claim Attachment image category definition. Returns the list of Defs found.</summary>
		private List<Def> CheckImageCatDefs() {
			//Filter down to image categories that have been marked as Claim Attachment.
			return Defs.GetCatList((int)DefCat.ImageCats).ToList().FindAll(x => x.ItemValue.Contains("C")&&!x.IsHidden);
		}

		///<summary>A helper method that does the actual validation of the claim. 
		///Can be called elsewhere in this form.</summary>
		private bool ValidateClaimHelper() {
			try {
				ClaimConnect.ValidateClaimResponse response=ClaimConnect.ValidateClaim(_claimCur,true);
				if(response._isValidClaim) {
					textClaimStatus.Text="The claim is valid.";
					return true;
				}
				//Otherwise the claim must have errors, display them to the user.
				StringBuilder strBuild=new StringBuilder();
				for(int i=0;i<response.ValidationErrors.Length;i++) {
					strBuild.AppendLine(response.ValidationErrors[i]);
				}
				textClaimStatus.Text=strBuild.ToString();
				return false;
			}
			catch(ODException ex) {
				textClaimStatus.Text=ex.Message;
				return false;
			}
			catch(Exception ex) {
				textClaimStatus.Text=ex.Message;
				return false;
			}
		}
		
		///<summary>Creates the ClaimAttach objects for each item in the grid and associates it to the given claim.
		///The parameter path should be the full path to the image and fileName should simply be the file name by itself.</summary>
		private ClaimAttach CreateClaimAttachment(string fileNameDisplay,string fileNameActual) {
			ClaimAttach claimAttachment=new ClaimAttach();
			claimAttachment.DisplayedFileName=fileNameDisplay;
			claimAttachment.ActualFileName=fileNameActual;
			claimAttachment.ClaimNum=_claimCur.ClaimNum;
			return claimAttachment;
		}

		private void buttonSnipTool_Click(object sender,EventArgs e) {
			this.WindowState=FormWindowState.Minimized;
			Image snip=FormODSnippingTool.Snip();
			ShowImageAttachmentItemEdit(snip);
			this.WindowState=FormWindowState.Normal;
		}

		private void ShowImageAttachmentItemEdit(Image img) {
			if(img==null) {
				return;
			}
			FormClaimAttachmentItemEdit form=new FormClaimAttachmentItemEdit(img);
			form.ShowDialog();
			if(form.DialogResult==DialogResult.OK) {
				_listImageAttachments.Add(form.ImageAttachment);
				FillGrid();
			}
		}

		private void buttonAddImage_Click(object sender,EventArgs e) {
			string patFolder=ImageStore.GetPatientFolder(_claimPat,ImageStore.GetPreferredAtoZpath());
			OpenFileDialog fileDialog=new OpenFileDialog();
			fileDialog.Multiselect=false;
			fileDialog.InitialDirectory=patFolder;
			if(fileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			//The filename property is the entire path of the file.
			string selectedFile=fileDialog.FileName;
			if(selectedFile.EndsWith(".pdf")) {
				MessageBox.Show(this,"DentalXChange does not support PDF attachments.");
				return;
			}
			//There is purposely no validation that the user selected an image as that will be handled on Dentalxchange's end.
			Image image;
			try {
				image=Image.FromFile(selectedFile);
				ShowImageAttachmentItemEdit(image);
			}
			catch(System.IO.FileNotFoundException ex) {
				FriendlyException.Show(Lan.g(this,"The selected file at: "+selectedFile+" could not be found"),ex);
			}
			catch(System.OutOfMemoryException ex) {
				//Image.FromFile() will throw an OOM exception when the image format is invalid or not supported.
				//See MSDN if you have trust issues:  https://msdn.microsoft.com/en-us/library/stf701f5(v=vs.110).aspx
				FriendlyException.Show(Lan.g(this,"The file does not have a valid image format. Please try again or call support."+"\r\n"+ex.Message),ex);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"An error occurred. Please try again or call support.")+"\r\n"+ex.Message,ex);
			}
		}

		///<summary>Allows users to paste an image from their clipboard. Throws if the content on the clipboard is not a supported image format.</summary>
		private void ButPasteImage_Click(object sender,EventArgs e) {
			try{
				Image imageClipboard=Clipboard.GetImage();
				if(imageClipboard==null) {
					//Either an image doesn't exist on the clipboard, or
					// an image exists on the clipboard that cannot be converted to a bitmap, in which case imageClipboard will also be null.
					throw new ODException(Lan.g(this,"Could not find a valid image on the clipboard."));
				}
				ShowImageAttachmentItemEdit(imageClipboard);
			}
			catch(ODException ode) {
				MsgBox.Show(this,ode.Message);
			}
			catch(ExternalException ee) {
				FriendlyException.Show(Lan.g(this,"The clipboard could not be accessed. This typically occurs when the clipboard is being used by another process."),ee);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"An error occurred while accessing the clipboard. Please call support if the issue persists."),ex);
			}
		}

		///<summary>Allows the user to edit an existing ImageAttachment object.</summary>
		private void CellDoubleClick_EditImage(object sender,ODGridClickEventArgs e) {
			GridRow selectedRow=gridAttachedImages.ListGridRows[gridAttachedImages.GetSelectedIndex()];
			ClaimConnect.ImageAttachment selectedAttachment=(ClaimConnect.ImageAttachment)selectedRow.Tag;
			FormClaimAttachmentItemEdit FormCAIE=new FormClaimAttachmentItemEdit(selectedAttachment.Image
				,selectedAttachment.ImageFileNameDisplay,selectedAttachment.ImageDate,selectedAttachment.ImageType);
			FormCAIE.ShowDialog();
			if(FormCAIE.DialogResult==DialogResult.OK) {//Update row
				_listImageAttachments[gridAttachedImages.GetSelectedIndex()]=FormCAIE.ImageAttachment;
				FillGrid();
			}
		}

		private void ContextMenu_ItemClicked(object sender,ToolStripItemClickedEventArgs e) {
			ToolStripItem item=e.ClickedItem;
			if(item.Text=="Delete") {
				//Delete every selected row
				foreach(int selectedIndex in gridAttachedImages.SelectedIndices) {
					_listImageAttachments.RemoveAt(selectedIndex);
				}
				FillGrid();
			}
		}

		private void textNarrative_TextChanged(object sender,EventArgs e) {
			labelCharCount.Text = textNarrative.Text.Length+"/2000";
		}

		///<summary>Sends every attachment in the grid to DentalXChange. Sets the claims attachmentID to
		///the response from Dentalxchange. Will also prompt the user to re-validate the claim.</summary>
		private void CreateAndSendAttachments() {
			//Grab all ImageAttachments from the grid.
			List<ClaimConnect.ImageAttachment> listImagesToSend=new List<ClaimConnect.ImageAttachment>();
			for(int i=0;i<gridAttachedImages.ListGridRows.Count;i++) {
				listImagesToSend.Add((ClaimConnect.ImageAttachment)gridAttachedImages.ListGridRows[i].Tag);
			}
			AddAttachments(listImagesToSend);
		}

		///<summary>Mimics CreateAndSendAttachments() but is split out for simplicity as this method will be rarely called.
		///Sends every attachment in the grid one-by-one to DentalXChange. Sets the claims attachmentID to
		///the response from Dentalxchange for the first attachment sent. Will also prompt the user to re-validate the claim.</summary>
		private void BatchSendAttachments() {
			ClaimConnect.ImageAttachment attachment;
			for(int i=0;i<gridAttachedImages.ListGridRows.Count;i++) {
				attachment=((ClaimConnect.ImageAttachment)gridAttachedImages.ListGridRows[i].Tag);
				AddAttachments(new List<ClaimConnect.ImageAttachment>() { attachment });
			}
		}

		///<summary>Sends the passed in attachments to ClaimConnect.  Will set the attachment id to the claim if needed.</summary>
		private void AddAttachments(List<ClaimConnect.ImageAttachment> listAttachments) {
			if(string.IsNullOrWhiteSpace(_claimCur.AttachmentID)) {
				//If an attachment has not already been created, create one.
				string attachmentId=ClaimConnect.CreateAttachment(listAttachments, textNarrative.Text,_claimCur);
				//Update claim if attachmentID was set. Must happen here so that the validation will consider the new attachmentID.
				_claimCur.AttachmentID=attachmentId;
				//Set the claims attached flag to 'Misc' so that the attachmentID will write to the PWK segment 
				//when the claim is generated as an 837.
				if(string.IsNullOrEmpty(_claimCur.AttachedFlags)) {
					_claimCur.AttachedFlags="Misc";
				}
				else {//Comma delimited
					_claimCur.AttachedFlags=",Misc";
				}
			}
			else {//An attachment already exists for this claim.
				ClaimConnect.AddAttachment(_claimCur, listAttachments);
			}
			Claims.Update(_claimCur);
		}

		///<summary>Saves all images in the grid to the patient on the claim's directory in the images module. Also creates
		///a list of ClaimAttach objects to associate to the given claim.</summary>
		private void buttonOK_Click(object sender,EventArgs e) {
			//The user must create an image or narrative attachment before sending.
			if(gridAttachedImages.ListGridRows.Count==0 && textNarrative.Text.Trim().Length==0) {
				MsgBox.Show(this,"An image or narrative must be specified before continuing.");
				return;
			}
			try {
				CreateAndSendAttachments();
			}
			//Creating and sending Attachments will sometimes time out when an arbirtrarily large group of attachments are being sent, 
			//at which point each attachment should be sent individually.
			catch(TimeoutException ex) {
				ex.DoNothing();
				ODProgress.ShowAction(() => { BatchSendAttachments(); },"Sending attachments timed out. Attempting to send individually. Please wait.");
			}
			catch(ODException ex) {
				//ODExceptions should already be Lans.g when throwing meaningful messages.
				//If they weren't translated, the message was from a third party and shouldn't be translated anyway.
				MessageBox.Show(ex.Message);
				return;
			}
			//Validate the claim, if it isn't valid let the user decide if they want to continue
			if(!ValidateClaimHelper()) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"There were errors validating the claim, would you like to continue?")) {
					return;
				}
			}
			//Used for determining which category to save the image attachments to. 0 will save the image to the first category in the Images module.
			long imageTypeDefNum=0;
			Def defClaimAttachCat=CheckImageCatDefs().FirstOrDefault();
			if(defClaimAttachCat!=null) {
				imageTypeDefNum=defClaimAttachCat.DefNum;
			}
			else {//User does not have a Claim Attachment image category, just use the first image category available.
				imageTypeDefNum=Defs.GetCatList((int)DefCat.ImageCats).FirstOrDefault(x => !x.IsHidden).DefNum;
			}
			List<ClaimAttach> listClaimAttachments=new List<ClaimAttach>();
			for(int i=0;i<gridAttachedImages.ListGridRows.Count;i++) {
				ClaimConnect.ImageAttachment imageRow=((ClaimConnect.ImageAttachment)gridAttachedImages.ListGridRows[i].Tag);
				if(PrefC.GetBool(PrefName.SaveDXCAttachments)) {
					Bitmap imageBitmap=new Bitmap(imageRow.Image);
					Document docCur=ImageStore.Import(imageBitmap,imageTypeDefNum,ImageType.Document,_claimPat);
					imageRow.ImageFileNameActual=docCur.FileName;
				}
				//Create attachment objects
				listClaimAttachments.Add(CreateClaimAttachment(imageRow.ImageFileNameDisplay,imageRow.ImageFileNameActual));
			}
			//Keep a running list of attachments sent to DXC for the claim. This will show in the attachments listbox.
			_claimCur.Attachments.AddRange(listClaimAttachments);
			Claims.Update(_claimCur);
			MsgBox.Show("Attachments sent successfully!");
			DialogResult=DialogResult.OK;
		}

		private void buttonCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
