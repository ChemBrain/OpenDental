using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness{
	///<summary>Used in the accounting section in chart of accounts.  Not related to patient accounts in any way.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true)]
	public class EClipboardSheetDef:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long EClipboardSheetDefNum;
		///<summary>FK to SheetDef.SheetDefNum.</summary>
		public long SheetDefNum;
		///<summary>FK to clinic.ClinicNum.  0 if no clinic or if default clinic.</summary>
		public long ClinicNum;
		///<summary>Indicates the acceptable amount of time that can pass since the last time the patient has filled this sheet out. Once this has
		///elapsed, if the EClipboardCreateMissingFormsOnCheckIn pref is turned on, this sheet will automatically be added to the patient
		///sheets to fill out when the patient is checked-in.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TimeSpanLong),XmlIgnore]
		public TimeSpan ResubmitInterval;
		///<summary>The order in which the patient will be asked to fill out this sheet.</summary>
		public int ItemOrder;

		///<summary></summary>
		public EClipboardSheetDef Clone() {
			return (EClipboardSheetDef)this.MemberwiseClone();
		}
	}
}