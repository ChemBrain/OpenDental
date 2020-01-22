using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using OpenDentBusiness;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class PayPlanLinks{
		#region Get Methods
		///<summary></summary>
		public static List<PayPlanLink> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlanLink>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM payplanlink WHERE PatNum = "+POut.Long(patNum);
			return Crud.PayPlanLinkCrud.SelectMany(command);
		}

		public static List<PayPlanLink> GetListForPayplan(long payplanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlanLink>>(MethodBase.GetCurrentMethod(),payplanNum);
			}
			string command=$"SELECT * FROM payplanlink WHERE PayPlanNum={POut.Long(payplanNum)}";
			return Crud.PayPlanLinkCrud.SelectMany(command);
		}

		public static List<PayPlanLink> GetForPayPlans(List<long> listPayPlans) {
			if(listPayPlans.IsNullOrEmpty()) {
				return new List<PayPlanLink>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlanLink>>(MethodBase.GetCurrentMethod(),listPayPlans);
			}
			string command=$"SELECT * FROM payplanlink WHERE PayPlanNum IN ({string.Join(",",listPayPlans.Select(x => POut.Long(x)))}) ";
			return Crud.PayPlanLinkCrud.SelectMany(command);
		}
		
		///<summary>Gets one PayPlanLink from the db.</summary>
		public static PayPlanLink GetOne(long payPlanLinkNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<PayPlanLink>(MethodBase.GetCurrentMethod(),payPlanLinkNum);
			}
			return Crud.PayPlanLinkCrud.SelectOne(payPlanLinkNum);
		}

		///<summary>Gets all of the payplanlink entries for the given fKey and linkType.</summary>
		public static List<PayPlanLink> GetForFKeyAndLinkType(long fKey,PayPlanLinkType linkType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PayPlanLink>>(MethodBase.GetCurrentMethod(),fKey,linkType);
			}
			string command=$"SELECT * FROM payplanlink WHERE payplanlink.FKey={POut.Long(fKey)} AND payplanlink.LinkType={POut.Int((int)linkType)} ";
			return Crud.PayPlanLinkCrud.SelectMany(command);
		}
		#endregion Get Methods
		#region Modification Methods
		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Passed in list contains current list and payPlanNum gets current 
		///list from DB.</summary>
		public static void Sync(List<PayPlanLink> listPayPlanLinks,long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listPayPlanLinks,payPlanNum);
				return;
			}
			List<PayPlanLink> listDB=GetListForPayplan(payPlanNum);
			Crud.PayPlanLinkCrud.Sync(listPayPlanLinks,listDB);
		}

		#region Insert
		///<summary></summary>
		public static long Insert(PayPlanLink payPlanLink){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				payPlanLink.PayPlanLinkNum=Meth.GetLong(MethodBase.GetCurrentMethod(),payPlanLink);
				return payPlanLink.PayPlanLinkNum;
			}
			return Crud.PayPlanLinkCrud.Insert(payPlanLink);
		}
		#endregion Insert
		#region Update
		///<summary></summary>
		public static void Update(PayPlanLink payPlanLink){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),payPlanLink);
				return;
			}
			Crud.PayPlanLinkCrud.Update(payPlanLink);
		}
		#endregion Update
		#region Delete
		///<summary></summary>
		public static void Delete(long payPlanLinkNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),payPlanLinkNum);
				return;
			}
			Crud.PayPlanLinkCrud.Delete(payPlanLinkNum);
		}
		#endregion Delete
		#endregion Modification Methods
		#region Misc Methods
		

		
		#endregion Misc Methods



	}
}