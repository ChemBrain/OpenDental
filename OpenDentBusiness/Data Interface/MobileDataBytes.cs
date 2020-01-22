using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeBase;
using PdfSharp.Pdf;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MobileDataBytes{
	private static Random _rand=new Random();

		#region Get Methods
		
		///<summary></summary>
		public static bool IsValidUnlockCode(string rawUnlockCode){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),rawUnlockCode);
			}
			return (TryGetForUnlockCode(rawUnlockCode)!=null);
		}

		///<summary></summary>
		public static MobileDataByte TryGetForUnlockCode(string rawUnlockCode){
			if(rawUnlockCode.IsNullOrEmpty()){
				return null;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<MobileDataByte>(MethodBase.GetCurrentMethod(),rawUnlockCode);
			}
			string command=$@"SELECT * FROM mobiledatabyte 
				WHERE RawBase64Code='{Convert.ToBase64String(Encoding.UTF8.GetBytes(rawUnlockCode))}'";
			return Crud.MobileDataByteCrud.SelectOne(command);
		}
		
		///<summary>Gets one MobileDataByte from the db.</summary>
		public static MobileDataByte GetOne(long mobileDataByteNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<MobileDataByte>(MethodBase.GetCurrentMethod(),mobileDataByteNum);
			}
			return Crud.MobileDataByteCrud.SelectOne(mobileDataByteNum);
		}
		#endregion Get Methods
		#region Modification Methods
		///<summary></summary>
		public static long Insert(MobileDataByte mobileDataByte){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				mobileDataByte.MobileDataByteNum=Meth.GetLong(MethodBase.GetCurrentMethod(),mobileDataByte);
				return mobileDataByte.MobileDataByteNum;
			}
			return Crud.MobileDataByteCrud.Insert(mobileDataByte);
		}
		///<summary></summary>
		public static void Update(MobileDataByte mobileDataByte){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),mobileDataByte);
				return;
			}
			Crud.MobileDataByteCrud.Update(mobileDataByte);
		}
		///<summary></summary>
		public static void Delete(long mobileDataByteNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),mobileDataByteNum);
				return;
			}
			Crud.MobileDataByteCrud.Delete(mobileDataByteNum);
		}

		///<summary>Called by eConnector to remove expired rows.</summary>
		public static void ClearExpiredRows(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			Db.NonQ($@"DELETE FROM mobiledatabyte 
				WHERE DateTimeEntry <={POut.Date(DateTime.Today.AddDays(-1))}
				OR DateTimeExpires <= {POut.Date(DateTime.Now)}"
			);
		}

		///<summary>Saves the given doc in the MobileDataByte table.
		///Returns true if no error occured, otherwise false and errorMsg is set.</summary>
		public static bool TryInsertPDF(PdfDocument doc,long patNum,string unlockCode,eActionType actionType
			,out long mobileDataByteNum,out string errorMsg,List<string> listTagVals=null)
		{
			errorMsg="";
			mobileDataByteNum=-1;
			try {
				string byteBase64;
				using(MemoryStream stream=new MemoryStream()) {
					doc.Save(stream);
					byteBase64=Convert.ToBase64String(stream.ToArray());
				}
				string tagData="";
				if(!listTagVals.IsNullOrEmpty()){
					tagData=Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join("###",listTagVals)));
				}
				mobileDataByteNum=Insert(new MobileDataByte() {
					PatNum=patNum,
					RawBase64Data=byteBase64,
					RawBase64Code=(unlockCode.IsNullOrEmpty()?"":Convert.ToBase64String(Encoding.UTF8.GetBytes(unlockCode))),
					RawBase64Tag=tagData,
					ActionType=actionType,
				});
			}
			catch(Exception ex){
				errorMsg=ex.Message;
			}
			return errorMsg.IsNullOrEmpty();
		}
		
		///<summary>Throws Exception.
		///Helper method that calls TryInsertPDF(...) for treatment plan PDFs.</summary>
		public static bool TryInsertTreatPlanPDF(PdfDocument doc,TreatPlan treatPlan,bool hasPracticeSig,string unlockCode
			,out string errorMsg,out MobileDataByte mobileDataByte){
			mobileDataByte=null;
			if(!MobileAppDevices.IsClinicSignedUpForEClipboard(Clinics.ClinicNum)) {
				throw new Exception($"This practice or clinic is not signed up for eClipboard.\r\nGo to eServices | Signup Portal to sign up.");
			}
			try{
				List<string> listTagValues=new List<string>() { treatPlan.Heading,treatPlan.TreatPlanNum.ToString(),hasPracticeSig.ToString(),
					treatPlan.DateTP.Ticks.ToString() };
				TryInsertPDF(doc,treatPlan.PatNum,unlockCode,eActionType.TreatmentPlan
					,out long mobileDataByteNum,out errorMsg,listTagValues
				);
				if(mobileDataByteNum!=-1){
					mobileDataByte=GetOne(mobileDataByteNum);
				}
			}
			catch(Exception ex){ 
				errorMsg=ex.Message;
			}
			return (errorMsg.IsNullOrEmpty() && mobileDataByte!=null);
		}

		///<summary>Code returned will not be in db. 
		///Returns null if no code could be generated in 100 tries that is not already in db.</summary>
		public static string GenerateUnlockCode(){
			string input="0123456789";
			string funcMakeUnlockCode() {
				return new string(Enumerable.Range(0,6).Select(x => input[_rand.Next(0,input.Length)]).ToArray());
			}
			string unlockCode=funcMakeUnlockCode();
			int tries=0;
			while(IsValidUnlockCode(unlockCode)){
				if(++tries>100){
					return null;
				}
				unlockCode=funcMakeUnlockCode();
			}
			return unlockCode;
		}

		#endregion Modification Methods
		#region Misc Methods
		

		
		#endregion Misc Methods
	}
}