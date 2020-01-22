using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>The patient does not want to recieve messages for a particular type of communication.</summary>
	[Serializable]
	[CrudTable(HasBatchWriteMethods=true)]
	public class CommOptOut:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long CommOptOutNum;
		///<summary>FK to patient.PatNum. The patient who is opting out of this form of communication.</summary>
		public long PatNum;
		///<summary>Enum:CommOptOutType The type of communication for which this patient does not want to receive messages.</summary>
		public CommOptOutType CommType;
		///<summary>Enum:CommOptOutMode The manner of message that the patient does not want to receive for this type of communication.</summary>
		public CommOptOutMode CommMode;

		///<summary></summary>
		public CommOptOut Clone() {
			return (CommOptOut)this.MemberwiseClone();
		}
		
	}

	public enum CommOptOutType {
		///<summary>0 - Should not be in the database.</summary>
		None,
		///<summary>1</summary>
		eConfirm,
		///<summary>2</summary>
		eReminder,
	}

	public enum CommOptOutMode {
		///<summary>0</summary>
		Text,
		///<summary>1</summary>
		Email,
	}
}




