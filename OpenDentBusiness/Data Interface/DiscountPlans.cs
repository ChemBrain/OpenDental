using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class DiscountPlans{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion


		///<summary></summary>
		public static List<DiscountPlan> GetAll(bool getHidden){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<DiscountPlan>>(MethodBase.GetCurrentMethod(),getHidden);
			}
			string command="SELECT * FROM discountplan";
			if(!getHidden) {
				command+=" WHERE IsHidden=0";
			}
			return Crud.DiscountPlanCrud.SelectMany(command);
		}

		/// <summary>Takes in a list of patnums and returns a dictionary of PatNum to DiscountPlan.FeeSchedNum pairs. Value is 0 if no discount plan exists</summary>
		public static SerializableDictionary<long,long> GetFeeSchedNumsByPatNums(List<long> listPatNums) {
			if(listPatNums.IsNullOrEmpty()) {
				return new SerializableDictionary<long,long>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetSerializableDictionary<long,long>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			string command="SELECT patient.PatNum,discountplan.FeeSchedNum "
				+ "FROM patient "
				+ "LEFT JOIN discountplan ON discountplan.DiscountPlanNum=patient.DiscountPlanNum "
				+ $"WHERE patient.PatNum IN ({string.Join(",",listPatNums)})";
			return Db.GetTable(command).Select()
				.ToSerializableDictionary(x => PIn.Long(x["PatNum"].ToString()),x => PIn.Long(x["FeeSchedNum"].ToString()));
		}

		///<summary>Gets one DiscountPlan from the db.</summary>
		public static DiscountPlan GetPlan(long discountPlanNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<DiscountPlan>(MethodBase.GetCurrentMethod(),discountPlanNum);
			}
			return Crud.DiscountPlanCrud.SelectOne(discountPlanNum);
		}

		///<summary></summary>
		public static long Insert(DiscountPlan discountPlan){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				discountPlan.DiscountPlanNum=Meth.GetLong(MethodBase.GetCurrentMethod(),discountPlan);
				return discountPlan.DiscountPlanNum;
			}
			return Crud.DiscountPlanCrud.Insert(discountPlan);
		}

		///<summary></summary>
		public static void Update(DiscountPlan discountPlan){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),discountPlan);
				return;
			}
			Crud.DiscountPlanCrud.Update(discountPlan);
		}

		///<summary>Sets DiscountPlanNum to 0 for specified PatNum.</summary>
		public static void DropForPatient(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			string command="UPDATE patient SET DiscountPlanNum=0 WHERE PatNum="+POut.Long(patNum);
			Db.NonQ(command);
		}

		///<summary>Changes the DiscountPlanNum of all patients that have _planFrom.DiscountPlanNum to _planInto.DiscountPlanNum</summary>
		public static void MergeTwoPlans(DiscountPlan planInto,DiscountPlan planFrom) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),planInto,planFrom);
				return;
			}
			string command="UPDATE patient SET DiscountPlanNum="+POut.Long(planInto.DiscountPlanNum)
				+" WHERE DiscountPlanNum="+POut.Long(planFrom.DiscountPlanNum);
			Db.NonQ(command);
			//Delete the discount plan from the database.
			Crud.DiscountPlanCrud.Delete(planFrom.DiscountPlanNum);
		}

		///<summary>Returns an empty list if planNum is 0.</summary>
		public static List<string> GetPatNamesForPlan(long planNum) {
			if(planNum==0) {
				return new List<string>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod(),planNum);
			}
			string command="SELECT LName,FName FROM patient WHERE DiscountPlanNum="+POut.Long(planNum)+" "+
				"AND PatStatus NOT IN("+POut.Int((int)PatientStatus.Deleted)+","+POut.Int((int)PatientStatus.Deceased)+") ";
			//No Preferred or MiddleI needed because this logic needs to match FormInsPlan.
			return Db.GetTable(command).Select().Select(x => Patients.GetNameLFnoPref(x["LName"].ToString(),x["FName"].ToString(),"")).ToList();
		}

		///<summary>Returns an empty list if planNum is 0.</summary>
		public static int GetPatCountForPlan(long planNum) {
			if(planNum==0) {
				return 0;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),planNum);
			}
			string command="SELECT COUNT(PatNum) FROM patient WHERE DiscountPlanNum="+POut.Long(planNum)+" "+
				"AND PatStatus NOT IN("+POut.Int((int)PatientStatus.Deleted)+","+POut.Int((int)PatientStatus.Deceased)+") ";
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Returns a dictionary where key=DiscountPlanNum and value=count of patients for the DiscountPlanNum.
		///Returns an empty dictionary if the list of plan nums is empty.</summary>
		public static SerializableDictionary<long,int> GetPatCountsForPlans(List<long> listPlanNums) {
			if(listPlanNums.Count==0) {
				return new SerializableDictionary<long,int>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetSerializableDictionary<long,int>(MethodBase.GetCurrentMethod(),listPlanNums);
			}
			string command="SELECT DiscountPlanNum,COUNT(PatNum) PatCount FROM patient " +
				"WHERE DiscountPlanNum IN ("+string.Join(",",listPlanNums)+") " +
				"AND PatStatus NOT IN("+POut.Int((int)PatientStatus.Deleted)+","+POut.Int((int)PatientStatus.Deceased)+") "+
				"GROUP BY DiscountPlanNum";
			return Db.GetTable(command).Select()
				.ToSerializableDictionary(x => PIn.Long(x["DiscountPlanNum"].ToString()),x => PIn.Int(x["PatCount"].ToString()));
		}
	}
}