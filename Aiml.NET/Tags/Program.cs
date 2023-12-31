using System.Xml;

namespace Aiml.NET {
	public partial class TemplateNode {
		/// <summary>
		///     Returns the name and version of the AIML interpreter.
		/// </summary>
		/// <remarks>
		///     <para>This element has no content.</para>
		///     <para>This element is defined by the AIML 2.0 specification.</para>
		/// </remarks>
		public sealed class Program : TemplateNode {
			public override string Evaluate(RequestProcess process) {
				return "CBot's AIML plugin, version 1.0";
			}

			public static Program FromXml(XmlNode node, AimlLoader loader) {
				return new Program();  // The size tag supports no properties.
			}
		}
	}
}
