using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeBase {
	public static class ODUIExtensions {

		///<summary>Returns the tag of the selected item. The items in the ComboBox must be ODBoxItems.</summary>
		public static T GetSelected<T>(this ComboBox comboBox) {
			if(comboBox.SelectedItem is ODBoxItem<T>) {
				return (comboBox.SelectedItem as ODBoxItem<T>).Tag;
			}
			return default(T);
		}
		
		#region listBox
		///<summary>Sets the selected item(s) that match the func passed in. Will only work if the Items in the ListBox are ODBoxItems.</summary>
		///<param name="fSelectItem">A func that takes an object that is the same type as the ODBoxItems Tags and returns a bool, i.e.
		///x => x.ClinicNum==0.</param>
		public static void SetSelectedItem<T>(this ListBox listBox,Func<T,bool> fSelectItem) {
			for(int i=0;i<listBox.Items.Count;i++) {
				ODBoxItem<T> odBoxItem=listBox.Items[i] as ODBoxItem<T>;
				if(odBoxItem==null) {
					continue;
				}
				if(fSelectItem(odBoxItem.Tag)) {
					listBox.SetSelected(i,true);
				}
			}
		}

		///<summary>Sets the items for the listBox to the given items list.</summary>
		///<param name="fItemToString">Func that takes an object that is the same type as the ODBoxItems Tags and returns a string to be displayed for this item, i.e.
		///x => x.Abbr.</param>
		///<param name="fSelectItem">Optional func that takes an object that is the same type as the ODBoxItems Tags and returns a bool, i.e.
		///x => x.ClinicNum==0. Pass null if you don't want to set initial item(s) to be selected.</param>
		public static void SetItems<T>(this ListBox listBox,IEnumerable<T> items,Func<T,string> fItemToString=null,Func<T,bool> fSelectItem=null) {
			listBox.Items.Clear();
			fItemToString=fItemToString??(x => x.ToString());
			foreach(T item in items) {
				listBox.Items.Add(new ODBoxItem<T>(fItemToString(item),item));
			}
			if(fSelectItem!=null) {
				listBox.SetSelectedItem(fSelectItem);
			}
		}

		///<summary>Gets all the Tags for all ODBoxItems added to Items.</summary>
		public static List<T> AllTags<T>(this ListBox listBox) {
			return listBox.Items.AsEnumerable<ODBoxItem<T>>().Select(x => x.Tag).ToList();
		}

		///<summary>Returns true if a SelectedItem exists and the SelectedItem's Tag is of type T.</summary>
		public static bool HasSelectedTag<T>(this ListBox listBox) {
			if(listBox.SelectedItem==null) {
				return false;
			}
			return listBox.SelectedItem is ODBoxItem<T>;
		}

		///<summary>Returns the tag of the selected item. The items in the ListBox must be ODBoxItems.</summary>
		public static T GetSelected<T>(this ListBox listBox) {
			if(listBox.SelectedItem is ODBoxItem<T>) {
				return (listBox.SelectedItem as ODBoxItem<T>).Tag;
			}
			return default(T);
		}
		
		///<summary>Returns the tags of the selected items. The items in the ListoBox must be ODBoxItems.</summary>
		public static List<T> GetListSelected<T>(this ListBox listBox) {
			List<T> listSelected=new List<T>();
			foreach(object selectedItem in listBox.SelectedItems) {
				if(selectedItem is ODBoxItem<T>) {
					listSelected.Add((selectedItem as ODBoxItem<T>).Tag);
				}
			}
			return listSelected;
		}
		#endregion listBox
		

		///<summary>Gets all controls and their children controls recursively.</summary>
		public static IEnumerable<Control> GetAllControls(this Control control) {
			IEnumerable<Control> controls=control.Controls.OfType<Control>();
			return controls.SelectMany(GetAllControls).Concat(controls);
		}

		///<summary>Takes a number of elements from the end of the enumerable.</summary>
		public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source,int count) {
			return source.Skip(Math.Max(0,source.Count() - count));
		}

		///<summary>Sometimes ODProgress can cause other forms to open up behind other applications. Call this method to force this form to the front.
		///</summary>
		public static void ForceBringToFront(this Form form) {
			form.TopMost=true;
			Application.DoEvents();
			form.TopMost=false;
		}

		///<summary>Shows the form nonmodally then performs the given action when the form closes.</summary>
		public static void ShowThen(this Form form,Action onClose) {
			form.Show();
			form.FormClosed+=(sender,e) => onClose();
		}
		
	}

	


}
