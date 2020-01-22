using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsCore {
	public class ProgramPropertyT {

		///<summary>Generates a single Unit Test ProgramProperty. This method refreshes the cache.</summary>
		public static ProgramProperty CreateProgramProperty(long programNum,string propertyDesc,long clinicNum,string propertyValue="",
			string computerName="")
		{
			ProgramProperty prop=new ProgramProperty();
			prop.ProgramNum=programNum;
			prop.PropertyDesc=propertyDesc;
			prop.PropertyValue=propertyValue;
			prop.ComputerName=computerName;
			prop.ClinicNum=clinicNum;
			ProgramProperties.Insert(prop);
			ProgramProperties.RefreshCache();
			return prop;
		}

		///<summary>Deletes everything from the Unit Test ProgramProperty table.</summary>
		public static void ClearProgamPropertyTable() {
			string command="DELETE FROM programproperty WHERE ProgramPropertyNum > 0";
			DataCore.NonQ(command);
			ProgramProperties.RefreshCache();
		}
	}
}
