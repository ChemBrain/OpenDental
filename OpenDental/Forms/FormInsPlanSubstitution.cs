using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Drawing;
using OpenDental.UI;
using CodeBase;

namespace OpenDental {
	public partial class FormInsPlanSubstitution:ODForm {

		private InsPlan _insPlan=null;
		private List <ProcedureCode> _listAllProcCodes=null;
		///<summary>Codes which currently have a substitution code set. Changes made to the Codes will be updated as the changes are made.</summary>
		private List <ProcedureCode> _listSubstProcCodes=null;
		///<summary>Links currently in the database.  This list will be manipulated inside this form. Changes made within the form will not 
		///be saved until the user clicks "OK".</summary>
		private List <SubstitutionLink> _listSubstLinks=null;
		///<summary>Used to sync when saving changes.</summary>
		private List <SubstitutionLink> _listSubstLinksOld=null;
		///<summary>Used to fill the SubstitutionCondition DropDown.</summary>
		private List<string> _listSubConditions;
		///<summary>Holds the string value of the substitution code string before making changes to the cell.</summary>
		private string _oldText;

		public FormInsPlanSubstitution(InsPlan insPlan) {
			InitializeComponent();
			Lan.F(this);
			_insPlan=insPlan;
		}

		private void FormInsPlanSubstitution_Load(object sender,EventArgs e) {
			_listAllProcCodes=ProcedureCodes.GetAllCodes();
			_listSubstProcCodes=_listAllProcCodes.FindAll(x => !String.IsNullOrEmpty(x.SubstitutionCode));
			_listSubstLinks=SubstitutionLinks.GetAllForPlans(_insPlan.PlanNum);
			_listSubstLinksOld=_listSubstLinks.DeepCopyList<SubstitutionLink,SubstitutionLink>();
			_listSubConditions=new List<string>();
			for(int i=0;i<Enum.GetNames(typeof(SubstitutionCondition)).Length;i++) {
				_listSubConditions.Add(Lan.g("enumSubstitutionCondition",Enum.GetNames(typeof(SubstitutionCondition))[i]));
			}
			FillGridMain();
		}

		private void FillGridMain() {
			ProcedureCode selectedCode=gridMain.SelectedTag<ProcedureCode>();
			gridMain.BeginUpdate();
			gridMain.ListGridRows.Clear();
			if(gridMain.ListGridColumns.Count==0) {
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g(gridMain.TranslationName,"ProcCode"),90));
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g(gridMain.TranslationName,"AbbrDesc"),100));
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g(gridMain.TranslationName,"SubstOnlyIf"),100){ListDisplayStrings=_listSubConditions });//Dropdown combobox
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g(gridMain.TranslationName,"SubstCode"),90,true));//Can edit cell
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g(gridMain.TranslationName,"SubstDesc"),90));
				gridMain.ListGridColumns.Add(new GridColumn(Lan.g(gridMain.TranslationName,"InsOnly"),0));
			}
			//Add all substitution codes for procedure code level
			foreach(ProcedureCode procCode in _listSubstProcCodes) {
				SubstitutionLink subLink=_listSubstLinks.FirstOrDefault(x => x.CodeNum==procCode.CodeNum && x.PlanNum==_insPlan.PlanNum);
				if(subLink!=null) {
					//Procedure has a Substitution Link for this insplan(override). The procedure will be added in the next foreach loop.
					continue;
				}
				//procedure code level substitution code
				AddRow(gridMain,procCode);
			}
			//Add all substitution codes for insurance level
			foreach(SubstitutionLink subLink in _listSubstLinks) {
				ProcedureCode procCode=_listAllProcCodes.FirstOrDefault(x => x.CodeNum==subLink.CodeNum);
				if(procCode==null) {
					continue;//This shouldn't happen.
				}
				//Procedure has a Substitution Link for this insplan.
				AddRow(gridMain,procCode,subLink);
			}
			SortGridByProc(gridMain);
			gridMain.EndUpdate();
			//Try an reselect the procedure code that was already selected prior to refreshing the grid.
			if(selectedCode!=null) {
				int index=gridMain.ListGridRows.ToList().FindIndex(x => (x.Tag as ProcedureCode).CodeNum==selectedCode.CodeNum);
				if(index > -1) {
					gridMain.SetSelected(new Point(2,index));
				}
			}
		}

		private void AddRow(ODGrid grid,ProcedureCode procCode,SubstitutionLink subLink=null) {
			//Set all of the row values for the procedure code passed in.
			string enumSubstCondition=procCode.SubstOnlyIf.ToString();
			string subCode=procCode.SubstitutionCode;
			ProcedureCode procCodeSubst=_listAllProcCodes.FirstOrDefault(x => x.ProcCode==procCode.SubstitutionCode);
			string subCodeDescript=(procCodeSubst==null)?"":procCodeSubst.AbbrDesc;
			string insOnly="";
			if(subLink!=null) {
				//This procedure code has a SubstitutionLink for this insurance plan.
				//set the row values to the Insplan override.
				enumSubstCondition=subLink.SubstOnlyIf.ToString();
				subCode=subLink.SubstitutionCode;
				subCodeDescript="";
				insOnly="X";
				//Validate the subLink.SubstitutionCode. subCodeDescript blank if the substitution code is not valid.
				//User can enter an invalid procedure code if they want.
				if(!string.IsNullOrEmpty(subLink.SubstitutionCode)) {
					ProcedureCode procCodeSub=_listAllProcCodes.FirstOrDefault(x=>x.ProcCode==subLink.SubstitutionCode);
					if(procCodeSub!=null) {
						subCodeDescript=procCodeSub.AbbrDesc;
					}
				}
			}
			GridRow row=new GridRow();
			row.Cells.Add(procCode.ProcCode);
			row.Cells.Add(procCode.AbbrDesc);
			GridCell cell=new GridCell(Lan.g("enumSubstitutionCondition",enumSubstCondition));
			cell.ComboSelectedIndex=_listSubConditions.FindIndex(x => x==enumSubstCondition);
			row.Cells.Add(cell);
			row.Cells.Add(subCode);
			row.Cells.Add(subCodeDescript);
			row.Cells.Add(insOnly);
			row.Tag=procCode;
			grid.ListGridRows.Add(row);
		}

		///<summary>Opens FormProcCodes in SelectionMode. Creates a new SubstitutionLink for the selected Procedure.</summary>
		private void ButAdd_Click(object sender, EventArgs e){
			FormProcCodes FormP=new FormProcCodes();
			FormP.IsSelectionMode=true;
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			_listAllProcCodes=ProcedureCodes.GetAllCodes();//in case they added a new proc code
			ProcedureCode procSelected=_listAllProcCodes.FirstOrDefault(x=>x.CodeNum==FormP.SelectedCodeNum);
			if(procSelected==null) {
				return;//should never happen, just in case
			}
			//Valid procedure selected. Create a new SubstitutionLink.  The user will be able to add the substition code on the cell grid.
			SubstitutionLink subLink=new SubstitutionLink();
			subLink.CodeNum=procSelected.CodeNum;
			subLink.PlanNum=_insPlan.PlanNum;
			subLink.SubstOnlyIf=SubstitutionCondition.Always;
			subLink.SubstitutionCode="";//Set to blank. The user will be able to add in the cell grid
			//The substitution link will be synced on OK click.
			_listSubstLinks.Add(subLink);
			FillGridMain();
			//Set the substitution link we just added as selected. The X pos at 3 is the SubstCode column.
			gridMain.SetSelected(new Point(3,gridMain.ListGridRows.ToList().FindIndex(x => (x.Tag as ProcedureCode).CodeNum==subLink.CodeNum)));
		}

		///<summary>Changes the SubstOnlyIf after the user selects a new SubstitutionCondition.
		///If the user modifies a procedure level substitution code, a new SubstitutionLink will be added for the inplan(override).
		///The new SubstitutionLink will be added to _listDbSubstLinks</summary>
		private void gridMain_CellSelectionCommitted(object sender,ODGridClickEventArgs e) {
			if(e.Col!=2 || e.Row<0 || e.Row>=gridMain.ListGridRows.Count) {//Return if not SubstOnlyIf column or invalid row
				return;
			}
			//Get the grid tag
			ProcedureCode procCode=gridMain.ListGridRows[e.Row].Tag as ProcedureCode;
			if(procCode==null) {
				return;
			}
			//Get the selected substitution condition.
			SubstitutionCondition selectedCondition=(SubstitutionCondition)_listSubConditions.IndexOf(gridMain.ListGridRows[e.Row].Cells[e.Col].Text);
			//Get the SubstitutionLink if one exist
			SubstitutionLink subLink=_listSubstLinks.FirstOrDefault(x => x.CodeNum==procCode.CodeNum);
			if(subLink!=null) {//Ins level sub code
				subLink.SubstOnlyIf=selectedCondition;
			}
			else {//procedure code level substitution code
				//Changing the SubstitutionCondition will not update the procedure code level SubstitutionCondition. We will create an insplan override.
				//Create a new SubstitutionLink for ins plan override for this proccode.
				subLink=new SubstitutionLink();
				subLink.CodeNum=procCode.CodeNum;
				//SubstitutionCode will be the same as the procedure codes SubstitutionCode since all the user changed was the SubstitutionCondition
				subLink.SubstitutionCode=procCode.SubstitutionCode;
				subLink.PlanNum=_insPlan.PlanNum;
				subLink.SubstOnlyIf=selectedCondition;
				_listSubstLinks.Add(subLink);
			}
			FillGridMain();
		}

		///<summary>Sets _oldText. Used in gridMain_CellLeave to check whether the text changed when leaving the cell.</summary>
		private void gridMain_CellEnter(object sender,ODGridClickEventArgs e) { 
			_oldText=gridMain.ListGridRows[e.Row].Cells[e.Col].Text;
		}

		///<summary>Changes the SubstitutionCode to what is entered in the cell. 
		///If the user modifies a procedure level substitution code, a new SubstitutionLink will be added for the inplan(override).
		///The new SubstitutionLink will be added to _listDbSubstLinks</summary>
		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			if(e.Col!=3 || e.Row<0) {//Return if not substitution code column or invalid row
				return;
			}
			ProcedureCode procCode=gridMain.ListGridRows[e.Row].Tag as ProcedureCode;
			if(procCode==null) {
				return;
			}
			string newText=gridMain.ListGridRows[e.Row].Cells[e.Col].Text;
			if(_oldText==newText) {
				return;
			}
			//Substitution code changed. 
			//We will not validate the new substitution code.
			//Get SubstitutionLink for ins level substitution code if one exist.
			SubstitutionLink subLink=_listSubstLinks.FirstOrDefault(x => x.CodeNum==procCode.CodeNum);
			if(subLink!=null) {//Ins level sub code
				subLink.SubstitutionCode=newText;
			}
			else {//procedure code level substitution code
				//We will not update the procedure code level substitution code. We will create an insplan override.
				//Create a new SubstitutionLink for ins plan override for this proccode.
				subLink=new SubstitutionLink();
				subLink.CodeNum=procCode.CodeNum;
				subLink.SubstitutionCode=newText;
				subLink.PlanNum=_insPlan.PlanNum;
				subLink.SubstOnlyIf=procCode.SubstOnlyIf;
				_listSubstLinks.Add(subLink);
			}
			FillGridMain();
		}

		private void SortGridByProc(ODGrid grid) {
			List<GridRow> listRows=grid.ListGridRows.ToList();
			listRows=listRows.OrderBy(x => ((ProcedureCode)x.Tag).ProcCode).ToList();
			grid.BeginUpdate();
			grid.ListGridRows.Clear();
			foreach(GridRow row in listRows) {
				grid.ListGridRows.Add(row);
			}
			grid.EndUpdate();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a substitution code first.");
				return;
			}
			ProcedureCode procCode=gridMain.SelectedTag<ProcedureCode>();
			SubstitutionLink subLink=_listSubstLinks.FirstOrDefault(x => x.CodeNum==procCode.CodeNum);
			if(subLink==null) {//User selected a procedure code level substitution code. Cannot delete
				MsgBox.Show(this,"Cannot delete a global substitution code.");
				return;
			}
			string msgText="Delete the selected insurance specific substitution code?\r\nDeleting the insurance specific substitution code will default to "
				+"the global substitution code for procedure";
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,Lan.g(this,msgText)+" \""+procCode.ProcCode+"\".")) {
				return;
			}
			_listSubstLinks.Remove(subLink);
			FillGridMain();
		}

		///<summary>Syncs _listDbSubstLinks and _listDbSubstLinksOld. Does not modify any of the procedure level SubstitutionCodes.</summary>
		private void butOK_Click(object sender,EventArgs e) {
			string msgText=Lan.g(this,"You have chosen to exclude all substitution codes.  "
				+"The checkbox option named 'Don't Substitute Codes (e.g. posterior composites)' "
				+"in the Other Ins Info tab of the Edit Insurance Plan window can be used to exclude all substitution codes.\r\n"
				+"Would you like to enable this option instead of excluding specific codes?");
			if(!_insPlan.CodeSubstNone
				&& _listSubstProcCodes.Select(x =>x.CodeNum).All(x => _listSubstLinks.Find(y => y.CodeNum==x)?.SubstOnlyIf==SubstitutionCondition.Never)
				&& MessageBox.Show(this,msgText,null,MessageBoxButtons.YesNo)==DialogResult.Yes)
			{
				_insPlan.CodeSubstNone=true;
			}
			//No need to make changes to the substitution codes at the procedure level.
			//Only syncing insurance level substitution links.  Changes made to the _listSubstLinks have been made already.
			//All we need to do know is sync the changes made with _listSubstLinksOld.
			SubstitutionLinks.Sync(_listSubstLinks,_listSubstLinksOld);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	
	}
}