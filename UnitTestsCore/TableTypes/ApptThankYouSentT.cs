using OpenDentBusiness;

namespace UnitTestsCore {
	public class ApptThankYouSentT {
		///<summary>Deletes everything from the apptremindersent table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearApptThankYouSentTable() {
			string command="DELETE FROM apptthankyousent WHERE ApptThankYouSentNum > 0";
			DataCore.NonQ(command);
		}
	}
}
