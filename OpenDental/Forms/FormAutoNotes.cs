using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using System.IO;
using Newtonsoft.Json;

namespace OpenDental {
	/// <summary>
	/// </summary>
	public class FormAutoNotes:ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.TreeView treeNotes;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.IContainer components;
		public AutoNote AutoNoteCur;
		private Label labelSelection;
		private ImageList imageListTree;
		private CheckBox checkCollapse;
		public bool IsSelectionMode;
		///<summary>On load, the UserOdPref that contains the comma delimited list of expanded category DefNums is retrieved from the database.  On close
		///the UserOdPref is updated with the current expanded DefNums.</summary>
		private UserOdPref _userOdCurPref;
		private UI.Button butExport;
		private UI.Button butImport;

		///<summary></summary>
		public FormAutoNotes(){
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ){
			if( disposing )
			{
				if(components != null)	{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoNotes));
			this.imageListTree = new System.Windows.Forms.ImageList(this.components);
			this.labelSelection = new System.Windows.Forms.Label();
			this.treeNotes = new System.Windows.Forms.TreeView();
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.checkCollapse = new System.Windows.Forms.CheckBox();
			this.butExport = new OpenDental.UI.Button();
			this.butImport = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// imageListTree
			// 
			this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
			this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListTree.Images.SetKeyName(0, "imageFolder");
			this.imageListTree.Images.SetKeyName(1, "imageText");
			// 
			// labelSelection
			// 
			this.labelSelection.Location = new System.Drawing.Point(15, 4);
			this.labelSelection.Name = "labelSelection";
			this.labelSelection.Size = new System.Drawing.Size(268, 14);
			this.labelSelection.TabIndex = 8;
			this.labelSelection.Text = "Select an Auto Note by double clicking.";
			this.labelSelection.Visible = false;
			// 
			// treeNotes
			// 
			this.treeNotes.AllowDrop = true;
			this.treeNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeNotes.HideSelection = false;
			this.treeNotes.ImageIndex = 1;
			this.treeNotes.ImageList = this.imageListTree;
			this.treeNotes.Indent = 12;
			this.treeNotes.Location = new System.Drawing.Point(18, 21);
			this.treeNotes.Name = "treeNotes";
			this.treeNotes.SelectedImageIndex = 1;
			this.treeNotes.Size = new System.Drawing.Size(307, 641);
			this.treeNotes.TabIndex = 2;
			this.treeNotes.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeNotes_ItemDrag);
			this.treeNotes.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeNotes_MouseDoubleClick);
			this.treeNotes.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeNotes_DragDrop);
			this.treeNotes.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeNotes_DragEnter);
			this.treeNotes.DragOver += new System.Windows.Forms.DragEventHandler(this.treeNotes_DragOver);
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(340, 637);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(79, 26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(340, 320);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 26);
			this.butAdd.TabIndex = 7;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// checkCollapse
			// 
			this.checkCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkCollapse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCollapse.Location = new System.Drawing.Point(340, 21);
			this.checkCollapse.Name = "checkCollapse";
			this.checkCollapse.Size = new System.Drawing.Size(79, 20);
			this.checkCollapse.TabIndex = 227;
			this.checkCollapse.Text = "Collapse All";
			this.checkCollapse.UseVisualStyleBackColor = true;
			this.checkCollapse.CheckedChanged += new System.EventHandler(this.checkCollapse_CheckedChanged);
			// 
			// butExport
			// 
			this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butExport.Location = new System.Drawing.Point(340, 352);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(79, 26);
			this.butExport.TabIndex = 228;
			this.butExport.Text = "Export";
			this.butExport.UseVisualStyleBackColor = true;
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// butImport
			// 
			this.butImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butImport.Location = new System.Drawing.Point(340, 384);
			this.butImport.Name = "butImport";
			this.butImport.Size = new System.Drawing.Size(79, 26);
			this.butImport.TabIndex = 229;
			this.butImport.Text = "Import";
			this.butImport.UseVisualStyleBackColor = true;
			this.butImport.Click += new System.EventHandler(this.butImport_Click);
			// 
			// FormAutoNotes
			// 
			this.ClientSize = new System.Drawing.Size(431, 675);
			this.Controls.Add(this.butImport);
			this.Controls.Add(this.butExport);
			this.Controls.Add(this.checkCollapse);
			this.Controls.Add(this.labelSelection);
			this.Controls.Add(this.treeNotes);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(447, 414);
			this.Name = "FormAutoNotes";
			this.ShowInTaskbar = false;
			this.Text = "Auto Notes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAutoNotes_FormClosing);
			this.Load += new System.EventHandler(this.FormAutoNotes_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormAutoNotes_Load(object sender, System.EventArgs e) {
			if(IsSelectionMode) {
				butAdd.Visible=false;
				labelSelection.Visible=true;
			}
			_userOdCurPref=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.AutoNoteExpandedCats).FirstOrDefault();
			AutoNoteL.FillListTree(treeNotes,_userOdCurPref);
		}

		/// <summary>Helper method that sets up the SaveFileDialog form and then returns that </summary>
		private OpenFileDialog ImportDialogSetup() {
			OpenFileDialog openDialog=new OpenFileDialog();
			openDialog.Multiselect=false;
			openDialog.Filter="JSON files(*.json)|*.json";
			openDialog.InitialDirectory=Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			return openDialog;
		}

		///<summary>Returns whether or not a node can be moved.
		///isSourceDef dictates whether the nodeCur is a definition or an autonote.
		///This is important because definitions are psuedo-directories and we need to guard against circular loops</summary>
		private bool IsValidDestination(TreeNode nodeCur,TreeNode nodeDest,bool isSourceDef) {
			//Null check just in case, destination node is already the parent of the selected node
			if(nodeCur==null || nodeCur.Parent==nodeDest) {
				return false;
			}
			//If the selected node is an auto note, it can move anywhere.
			//It was determined that the parent is different, so the node will actually move somewhere.
			if(!isSourceDef) {
				return true;
			}
			//The nodeCur is a definition, so we need to make sure it isn't trying to be moved to a decendant of itself.
			if(nodeDest!=null && nodeDest.FullPath.StartsWith(nodeCur.FullPath)) {
				return false;
			}
			return true;
		}

		private void treeNotes_MouseDoubleClick(object sender,TreeNodeMouseClickEventArgs e) {
			if(e.Node==null || !(e.Node.Tag is AutoNote)) {
				return;
			}
			AutoNote noteCur=((AutoNote)e.Node.Tag).Copy();
			if(IsSelectionMode) {
				AutoNoteCur=noteCur;
				DialogResult=DialogResult.OK;
				return;
			}
			FormAutoNoteEdit FormA=new FormAutoNoteEdit();
			FormA.IsNew=false;
			FormA.AutoNoteCur=noteCur;
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				AutoNoteL.FillListTree(treeNotes,_userOdCurPref);
			}
		}

		private TreeNode _grayNode=null;//only used in treeNotes_DragOver to reduce flickering.

		private void treeNotes_DragOver(object sender,DragEventArgs e) {
			Point pt=treeNotes.PointToClient(new Point(e.X,e.Y));
			TreeNode nodeSelected=treeNotes.GetNodeAt(pt);
			if(_grayNode!=null && _grayNode!=nodeSelected) {
				_grayNode.BackColor=Color.White;
				_grayNode=null;
			}
			if(nodeSelected!=null && nodeSelected.BackColor!=Color.LightGray) {
				nodeSelected.BackColor=Color.LightGray;
				_grayNode=nodeSelected;
			}
			if(pt.Y<25) {
				MiscUtils.SendMessage(treeNotes.Handle,277,0,0);//Scroll Up
			}
			else if(pt.Y>treeNotes.Height-25) {
				MiscUtils.SendMessage(treeNotes.Handle,277,1,0);//Scroll down.
			}
		}

		private void treeNotes_ItemDrag(object sender,ItemDragEventArgs e) {
			treeNotes.SelectedNode=(TreeNode)e.Item;
			DoDragDrop(e.Item,DragDropEffects.Move);
		}

		private void treeNotes_DragEnter(object sender,DragEventArgs e) {
			e.Effect=DragDropEffects.Move;
		}

		private void treeNotes_DragDrop(object sender,DragEventArgs e) {
			if(_grayNode!=null) {
				_grayNode.BackColor=Color.White;
			}
			if(!e.Data.GetDataPresent("System.Windows.Forms.TreeNode",false)) { 
				return; 
			}
			TreeNode sourceNode=(TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
			if(sourceNode==null || !(sourceNode.Tag is Def || sourceNode.Tag is AutoNote)) {
				return;
			}
			TreeNode topNodeCur=treeNotes.TopNode;
			if(treeNotes.TopNode==sourceNode && sourceNode.PrevVisibleNode!=null) {
				//if moving the topNode to another category, make the previous visible node the topNode once the move is successful
				topNodeCur=sourceNode.PrevVisibleNode;
			}
			Point pt=((TreeView)sender).PointToClient(new Point(e.X,e.Y));
			TreeNode destNode=((TreeView)sender).GetNodeAt(pt);
			if(destNode==null || !(destNode.Tag is Def || destNode.Tag is AutoNote)) {//moving to root node (category 0)
				if(sourceNode.Parent==null) {//already at the root node, nothing to do
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Move the selected "+(sourceNode.Tag is AutoNote?"Auto Note":"category")+" to the root level?")) {
					return;
				}
				if(sourceNode.Tag is Def) {
					((Def)sourceNode.Tag).ItemValue="";
				}
				else {//must be an AutoNote
					((AutoNote)sourceNode.Tag).Category=0;
				}
			}
			else {//moving to another folder (category)
				if(destNode.Tag is AutoNote) {
					destNode=destNode.Parent;//if destination is AutoNote, set destination to the parent, which is the category def node for the note
				}
				if(!IsValidDestination(sourceNode,destNode,sourceNode.Tag is Def)) {
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,
					"Move the selected "+(sourceNode.Tag is AutoNote?"Auto Note":"category")+(destNode==null?" to the root level":"")+"?"))
				{
					return;
				}
				//destNode will be null if a root AutoNote was selected as the destination
				long destDefNum=(destNode==null?0:((Def)destNode.Tag).DefNum);
				if(sourceNode.Tag is Def) {
					((Def)sourceNode.Tag).ItemValue=(destDefNum==0?"":destDefNum.ToString());//make a DefNum of 0 be a blank string in the db, not a "0" string
				}
				else {//must be an AutoNote
					((AutoNote)sourceNode.Tag).Category=destDefNum;
				}
			}
			if(sourceNode.Tag is Def) {
				Defs.Update((Def)sourceNode.Tag);
				DataValid.SetInvalid(InvalidType.Defs);
			}
			else {//must be an AutoNote
				AutoNotes.Update((AutoNote)sourceNode.Tag);
				DataValid.SetInvalid(InvalidType.AutoNotes);
			}
			treeNotes.TopNode=topNodeCur;//if sourceNode was the TopNode and was moved, make the TopNode the previous visible node
			AutoNoteL.FillListTree(treeNotes,_userOdCurPref);
		}

		private void checkCollapse_CheckedChanged(object sender,System.EventArgs e) {
			AutoNoteL.SetCollapsed(treeNotes,checkCollapse.Checked);
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit)) {
				return;
			}
			long selectedDefNum=0;
			if(treeNotes.SelectedNode?.Tag is Def) {
				selectedDefNum=((Def)treeNotes.SelectedNode.Tag).DefNum;
			}
			else if(treeNotes.SelectedNode?.Tag is AutoNote) {
				selectedDefNum=((AutoNote)treeNotes.SelectedNode.Tag).Category;
			}
			FormAutoNoteEdit FormA=new FormAutoNoteEdit();
			FormA.IsNew=true;
			FormA.AutoNoteCur=new AutoNote() { Category=selectedDefNum };
			FormA.ShowDialog();
			if(FormA.DialogResult!=DialogResult.OK) {
				return;
			}
			treeNotes.SelectedNode?.Expand();//expanding an AutoNote has no effect, and if nothing selected nothing to expand
			AutoNoteL.FillListTree(treeNotes,_userOdCurPref);
			if((FormA.AutoNoteCur?.AutoNoteNum??0)>0) {//select the newly added note in the tree
				treeNotes.SelectedNode=treeNotes.Nodes.OfType<TreeNode>().SelectMany(x => AutoNoteL.GetNodeAndChildren(x)).Where(x => x.Tag is AutoNote)
					.FirstOrDefault(x => ((AutoNote)x.Tag).AutoNoteNum==FormA.AutoNoteCur.AutoNoteNum);
				treeNotes.SelectedNode?.EnsureVisible();
				treeNotes.Focus();
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void butExport_Click(object sender,System.EventArgs e) {
			FormAutoNoteExport export=new FormAutoNoteExport();
			export.ShowDialog();
		}

		private void butImport_Click(object sender,EventArgs e) {
			try {
				using(OpenFileDialog openDialog=ImportDialogSetup()) {
					if(openDialog.ShowDialog()!=DialogResult.OK) {
						return; //User cancelled out of OpenFileDialog
					}
					string fileContents=File.ReadAllText(openDialog.FileName);
					TransferableAutoNotes import=JsonConvert.DeserializeObject<TransferableAutoNotes>(fileContents);
					AutoNoteControls.RemoveDuplicatesFromList(import.AutoNoteControls,import.AutoNotes);
					AutoNoteControls.InsertBatch(import.AutoNoteControls);
					AutoNotes.InsertBatch(import.AutoNotes);
					DataValid.SetInvalid(InvalidType.AutoNotes);
					AutoNoteL.FillListTree(treeNotes,_userOdCurPref);
					SecurityLogs.MakeLogEntry(Permissions.AutoNoteQuickNoteEdit,0,
						$"Auto Note Import. {import.AutoNotes.Count} new Auto Notes, {import.AutoNoteControls.Count} new Prompts");
					MsgBox.Show(Lans.g(this,"Auto Notes successfully imported!")+"\r\n"+import.AutoNotes.Count+" "+Lans.g(this,"new Auto Notes")
						+"\r\n"+import.AutoNoteControls.Count+" "+Lans.g(this,"new Prompts"));
				}
			}
			catch(Exception err) {
				FriendlyException.Show(Lans.g(this,"Auto Note(s) failed to import."),err);
			}
		}

		private void FormAutoNotes_FormClosing(object sender,FormClosingEventArgs e) {
			//store the current node expanded state for this user
			List<long> listExpandedDefNums=treeNotes.Nodes.OfType<TreeNode>()
				.SelectMany(x => AutoNoteL.GetNodeAndChildren(x,true))
				.Where(x => x.IsExpanded)
				.Select(x => ((Def)x.Tag).DefNum)
				.Where(x => x>0).ToList();
			if(_userOdCurPref==null) {
				UserOdPrefs.Insert(new UserOdPref() {
					UserNum=Security.CurUser.UserNum,
					FkeyType=UserOdFkeyType.AutoNoteExpandedCats,
					ValueString=string.Join(",",listExpandedDefNums)
				});
			}
			else {
				_userOdCurPref.ValueString=string.Join(",",listExpandedDefNums);
				UserOdPrefs.Update(_userOdCurPref);
			}
		}
	}

	///<summary>Sorting class used to sort a MethodInfo list by Name.</summary>
	public class NodeSorter:IComparer<TreeNode> {

		public int Compare(TreeNode x,TreeNode y) {
			if(x.Tag is Def && y.Tag is AutoNote) {
				return -1;
			}
			if(x.Tag is AutoNote && y.Tag is Def) {
				return 1;
			}
			if(x.Tag is Def && y.Tag is Def) {
				Def defX=(Def)x.Tag;
				Def defY=(Def)y.Tag;
				if(defX.ItemOrder!=defY.ItemOrder) {
					return defX.ItemOrder.CompareTo(defY.ItemOrder);
				}
			}
			//either both nodes are AutoNote nodes or both are Def nodes and both have the same ItemOrder (shouldn't happen), sort alphabetically
			return x.Text.CompareTo(y.Text);
		}
	}

}
