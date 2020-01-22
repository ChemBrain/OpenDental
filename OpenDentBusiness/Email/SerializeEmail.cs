using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OpenDentBusiness.Email {
	using Extensions;

	///<summary>Convert EmailRequest and EmailResult to/from string.</summary>
	public class Serializer {
		#region EmailRequest
		public static string SerializeRequest(EmailRequest emailRequest) {
			return emailRequest.Serialize();
		}
		public static EmailRequest DeserializeRequest(string xml) {
			return xml.Deserialize<EmailRequest>();
		}
		#endregion

		#region EmailResult
		public static string SerializeResult(EmailResult emailResult) {
			return emailResult.Serialize();
		}
		public static EmailResult DeserializeResult(string xml) {
			return xml.Deserialize<EmailResult>();
		}
		#endregion
	}
}

namespace OpenDentBusiness.Email.Extensions {
	public static class EmailExtensions {
		public static T Deserialize<T>(this string toDeserialize) {
			XmlSerializer xmlSerializer=new XmlSerializer(typeof(T));
			using(StringReader textReader=new StringReader(toDeserialize)) {
				T objEscaped=(T)xmlSerializer.Deserialize(textReader);
				return (T)XmlConverter.XmlUnescapeRecursion(typeof(T),objEscaped);
			}
		}

		public static string Serialize<T>(this T toSerialize) {
			T escapedObj=(T)XmlConverter.XmlEscapeRecursion(typeof(T),toSerialize);
			XmlSerializer xmlSerializer=new XmlSerializer(typeof(T));
			StringWriter writer=new StringWriter();
			using(XmlWriter textWriter=XmlWriter.Create(writer,new XmlWriterSettings { Indent=false,NewLineHandling=NewLineHandling.None })) {
				xmlSerializer.Serialize(textWriter,toSerialize);
				return writer.ToString();
			}
		}
	}
}