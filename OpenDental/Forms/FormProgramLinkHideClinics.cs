using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormProgramLinkHideClinics:ODForm {

		
		private readonly long _progNumCur;

		///<summary>A list of ProgramProperties populated by the listboxHiddenClinics listbox.</summary>
		private List<ProgramProperty> _listProgramPropertiesHiddenClinics {
			get {
				//Based on listboxHiddenClinics, create new ProgramProperty accessible in FormProgramLinkEdit
				return listboxHiddenClinics.AllTags<Clinic>().Select(x => new ProgramProperty() {
					ProgramNum=_progNumCur,
					PropertyDesc=ProgramProperties.PropertyDescs.ClinicHideButton,//Give it description we can use to delimit on later.
					ClinicNum=x.ClinicNum,
				}).ToList();
			}
		}

		public FormProgramLinkHideClinics(Program programCur,List<ProgramProperty> listProgProp,List<Clinic> listUserClinics) {
			InitializeComponent();
			Lan.F(this);
			showClinicStateWarning(Security.CurUser.ClinicIsRestricted);
			checkOrderAlphabetical.Checked=PrefC.GetBool(PrefName.ClinicListIsAlphabetical);
			_progNumCur=programCur.ProgramNum;
			List<Clinic> listVisibleButtonsForClinics=listUserClinics.Select(x => x.Copy()).ToList();//Copy so we don't affect the source list.
			List<Clinic> listHiddenButtonsForClinics=new List<Clinic>();
			if(!listProgProp.IsNullOrEmpty()) {
				//If a ProgramProperty exists within listProgProp (hide for these clinics) then we move the correpsonding clinic to listHideButtonsForClinics
				for(int i=listVisibleButtonsForClinics.Count-1;i>=0;i--) {//Count backwards to allow for clean removal from lists.
					if(listProgProp.Any(x => x.ClinicNum==listVisibleButtonsForClinics[i].ClinicNum)) {//Means the buttons are hidden for this clinic.
						listHiddenButtonsForClinics.Add(listVisibleButtonsForClinics[i]);//If removing from the "Visible" list then it goes into the "Hidden".
						listVisibleButtonsForClinics.Remove(listVisibleButtonsForClinics[i]);//Then remove from "Visible". (add/remove order matters).
					}
				}
			}
			//Populate both listboxes based on above lists.
			listHiddenButtonsForClinics.Sort(NaturalSort);//For Alphabetical Sort
			listVisibleButtonsForClinics.Sort(NaturalSort);
			listboxVisibleClinics.SetItems(listVisibleButtonsForClinics,x => x.Abbr);//Sets the buttons based on sorted lists
			listboxHiddenClinics.SetItems(listHiddenButtonsForClinics,x => x.Abbr);
		}

		///<summary>Show label warning the user clinics may not be visible due to clinic restriction if they are in fact clinic restricted.</summary>
		private void showClinicStateWarning(bool isUserClinicRestricted) {
			if(!isUserClinicRestricted) {
				labelClinicStateWarning.Visible=false;
			}
		}

		///<summary>Syncs any changes made by the user to the list of Program Properties that indicates this Program Link's button should be hidden
		///per clinic.  Only syncs changes made to ProgramProperties for clinics the user has access to.</summary>
		private void SyncHiddenProgramProperties() {
			//Get the users total list of unrestricted clinics, then acquire their list of ProgramProperties so we can tell which PL buttons 
			//should be hidden based upon the ProgramProperty PropertyDesc indicator. 
			List<Clinic> listUserClinics=Clinics.GetForUserod(Security.CurUser,doIncludeHQ:true,hqClinicName:Lan.g(this,"HQ"));
			//Get the cached list of button hiding ProgramProperties for clinics this user has access to, i.e. the "Old" list.
			List<ProgramProperty> listHiddenForUserOld=ProgramProperties.GetForProgram(_progNumCur)
				.Where(x => x.PropertyDesc==ProgramProperties.PropertyDescs.ClinicHideButton 
					&& x.ClinicNum.In(listUserClinics.Select(y => y.ClinicNum))).ToList();
			//Compares the old list of ProgramProperties to the new one, if a clinic exists in the old list but not the new list then it was deleted by the 
			//user and we remove it from the db.
			foreach(ProgramProperty propOld in listHiddenForUserOld) {
				if(!propOld.ProgramPropertyNum.In(_listProgramPropertiesHiddenClinics.Select(x => x.ProgramPropertyNum))) {//Clinic was Removed from List
					ProgramProperties.Delete(propOld);//Remove from ProgramProperty
				}
			}
			//Compares the new list of ProgramProperties to the old one, if a clinic exists in the new list but not the old one then it was added by the 
			//user and we should add it to the db.
			foreach(ProgramProperty propNew in _listProgramPropertiesHiddenClinics) {
				if(!propNew.ProgramPropertyNum.In(listHiddenForUserOld.Select(x => x.ProgramPropertyNum))) {//Clinic was Added to List
					ProgramProperties.Insert(propNew);//Insert ProgramProperty
				}
			}
		}

		///<summary>A sort to switch list sorting between ASCIIbetical and Alphabetical. This method is ClinicSort(...) from FormClinics.cs.</summary>
		private int NaturalSort(Clinic x,Clinic y) {
			if(x.ClinicNum==0) {
					return -1;
			}
			if(y.ClinicNum==0) {
					return 1;
			}
			int retval=0;
			if(checkOrderAlphabetical.Checked) {//order alphabetical by Abbr
				retval=x.Abbr.CompareTo(y.Abbr);
			}
			else {//not alphabetical, order by ItemOrder
				retval=x.ItemOrder.CompareTo(y.ItemOrder);
			}
			if(retval==0) {//if Abbr's are alphabetically the same, order alphabetical by Description
				retval=x.Description.CompareTo(y.Description);
			}
			if(retval==0) {//if Abbrs are the same and Descriptions are alphabetically the same, order by ClinicNum (guaranteed deterministic)
				retval=x.ClinicNum.CompareTo(y.ClinicNum);
			}
			return retval;
		}

		private void ListboxVisibleClinics_MouseClick(object sender,MouseEventArgs e) {
			listboxHiddenClinics.ClearSelected();
		}

		private void ListboxHiddenClinics_MouseClick(object sender,MouseEventArgs e) {
			listboxVisibleClinics.ClearSelected();
		}

		///<summary>Moving a clinic from Hidden to Visible</summary>
		private void butRight_Click(object sender,EventArgs e) {
			List<Clinic> listMoved=MoveClinics(listFrom:listboxHiddenClinics,listTo:listboxVisibleClinics);
			ReselectClinics(listboxVisibleClinics,listMoved);
		}

		/// <summary>Moving a clinic from Visible to Hidden</summary>
		private void butLeft_Click(object sender,EventArgs e) {
			List<Clinic> listMoved=MoveClinics(listFrom:listboxVisibleClinics,listTo:listboxHiddenClinics);
			ReselectClinics(listboxHiddenClinics,listMoved);
		}

		///<summary>Method reads in a ListBox to move items from, and another ListBox to move items into. It checks the first list's selected indices and then, if it 
		///finds a matching clinic it removes it from the "from" list and moves it to the "to" list.</summary>
		private List<Clinic> MoveClinics(ListBox listFrom,ListBox listTo) {
			if(listFrom.SelectedIndices.Count==0) {
				return new List<Clinic>();//If nothing is selected, change nothing so the UI doesn't refresh.
			}
			List<Clinic> listClinicsTo=listTo.AllTags<Clinic>();
			List<Clinic> listMoved=new List<Clinic>();
			//We go through the listFrom and find the SelectedIndicies. Then, remove them from the listFrom and add them to listTo after.
			foreach(int index in listFrom.SelectedIndices.AsEnumerable<int>().OrderByDescending(x => x)) {
				Clinic clinic=((ODBoxItem<Clinic>)listFrom.Items[index]).Tag;
				listFrom.Items.RemoveAt(index);
				listMoved.Add(clinic);
			}
			listClinicsTo.AddRange(listMoved);
			//Set box items again & deselect the listboxes
			listClinicsTo.Sort(NaturalSort);//Sort Alphabetically
			listTo.SetItems(listClinicsTo,x => x.Abbr);
			return listMoved;
		}

		///<summary>If the ordering has changed, update the list and maintain selection.</summary>
		private void CheckOrderAlphabetical_CheckedChanged(object sender,EventArgs e) {
			List<Clinic> listVisible=listboxVisibleClinics.AllTags<Clinic>();//Acquire the full list of tags for SetItems.
			List<Clinic> listHidden=listboxHiddenClinics.AllTags<Clinic>();
			List<Clinic> listVisibleSelected=listboxVisibleClinics.GetListSelected<Clinic>();//Aciquire the Selected tags for SetItems.
			List<Clinic> listHiddenSelected=listboxHiddenClinics.GetListSelected<Clinic>();
			listVisible.Sort(NaturalSort);//Sorts based on the status of the checkbox.
			listHidden.Sort(NaturalSort);
			listboxVisibleClinics.SetItems(listVisible,x => x.Abbr,x => x.ClinicNum.In(listVisibleSelected.Select(y => y.ClinicNum)));
			listboxHiddenClinics.SetItems(listHidden,x => x.Abbr,x => x.ClinicNum.In(listHiddenSelected.Select(y => y.ClinicNum)));
		}

		///<summary>Maintain the selection of the user when either moving items between sides or changing sort typyes.</summary>
		private void ReselectClinics(ListBox listBox,List<Clinic> listSelected) {
			listBox.SetSelectedItem<Clinic>(x => x.ClinicNum.In(listSelected.Select(y => y.ClinicNum)));
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//ProgramProperties will only sync in the case of the OK click, on a cancel they will not be saved.
			SyncHiddenProgramProperties();
			DialogResult=DialogResult.OK;
		}
	}
}