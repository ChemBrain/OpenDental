using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Xml;

namespace OpenDental {
	public partial class FormSupportStatus:ODForm {
		private string _regKey;
		public FormSupportStatus() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSupportStatus_Load(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			_regKey=PrefC.GetString(PrefName.RegistrationKey);
			textRegKey.Text=_regKey;
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent=true;
			settings.IndentChars=("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer = XmlWriter.Create(strbuild,settings)) {
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(_regKey);
				writer.WriteEndElement();
			}
			OpenDentBusiness.localhost.Service1 updateService=CustomerUpdatesProxy.GetWebServiceInstance();
			string result="";
			try {
				result=updateService.RequestRegKeyStatus(strbuild.ToString());
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show("Error: "+ex.Message);
				this.Close();
				return;
			}
			Cursor=Cursors.Default;
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				MessageBox.Show(node.InnerText,"Error");
				return;
			}
			node=doc.SelectSingleNode("//KeyDisabled");
			if(node!=null) {
				if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,true)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
				labelStatusValue.Text="DISABLED";
				labelStatusValue.ForeColor=Color.Red;
			}
			//Checking all three statuses in case RequestRegKeyStatus changes in the future
			node=doc.SelectSingleNode("//KeyEnabled");
			if(node!=null) {
				if(Prefs.UpdateBool(PrefName.RegistrationKeyIsDisabled,false)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
				labelStatusValue.Text="ENABLED";
				labelStatusValue.ForeColor=Color.Green;
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}
	}
}