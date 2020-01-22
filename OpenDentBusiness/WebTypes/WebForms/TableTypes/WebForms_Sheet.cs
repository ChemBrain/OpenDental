using System;
using OpenDentBusiness;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace OpenDentBusiness.WebTypes.WebForms {
	[Serializable]
	[CrudTable(IsMissingInGeneral=true,CrudLocationOverride=@"..\..\..\OpenDentBusiness\WebTypes\WebForms\Crud",NamespaceOverride="OpenDentBusiness.WebTypes.WebForms.Crud",CrudExcludePrefC=true)]
	public class WebForms_Sheet:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SheetID;
		///<summary>FK to customers.patient.PatNum.</summary>
		public long DentalOfficeID;
		///<summary>Copied from the SheetDef description.</summary>
		public string Description;
		///<summary>Enum:SheetTypeEnum  The type of sheet.  Only PatientForm, MedicalHistory, and Consent sheet types are supported in Web Forms.
		///!!!NOTE!!! The actual enum type associated to this field is incorrect by pointing to the SheetFieldType enum.
		///This cannot easily change due to backwards compatibility reasons.
		///E.g. Older versions are expecting the value in the XML payload to be a string representation of SheetFieldType values.
		///This means that there needs to be a backwards compatible fix put in place when this enum type gets corrected.</summary>
		public SheetFieldType SheetType;
		///<summary>The date and time of the sheet as it will be displayed in the commlog.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeSheet;
		///<summary>The default fontSize for the sheet.  The actual font must still be saved with each sheetField.</summary>
		public float FontSize;
		///<summary>The default fontName for the sheet.  The actual font must still be saved with each sheetField.</summary>
		public string FontName;
		///<summary>Width of each page in the sheet in pixels, 100 pixels per inch.</summary>
		public int Width;
		///<summary>Height of each page in the sheet in pixels, 100 pixels per inch.</summary>
		public int Height;
		///<summary>.</summary>
		public bool IsLandscape;
		///<summary>FK to clinic.ClinicNum. Used by webforms to limit the sheets displayed based on the currently selected clinic.</summary>
		public long ClinicNum;
		///<summary>If true then this Sheet has been designed for mobile and will be displayed as a mobile-friendly WebForm.</summary>
		public bool HasMobileLayout;
		///<summary>FK to sheetdef.SheetDefNum.  The SheetDef that was used to create this sheet.</summary>
		public long SheetDefNum;

		///<Summary></Summary>
		[CrudColumn(IsNotDbColumn=true)]
		[XmlIgnore]
		public List<WebForms_SheetField> SheetFields;

		public WebForms_Sheet(){

		}
		
    public WebForms_Sheet Copy(){
			return (WebForms_Sheet)this.MemberwiseClone();
		}

		///<summary>Used only for serialization purposes</summary>
		[XmlElement("SheetFields",typeof(WebForms_SheetField[]))]
		public WebForms_SheetField[] SheetFieldsXml {
			get {
				if(SheetFields==null) {
					return new WebForms_SheetField[0];
				}
				return SheetFields.ToArray();
			}
			set {
				SheetFields=new List<WebForms_SheetField>();
				for(int i=0;i<value.Length;i++) {
					SheetFields.Add(value[i]);
				}
			}
		}

	}
}