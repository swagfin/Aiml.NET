using System.Xml;

namespace Aiml.NET {
	public partial class TemplateNode {
		/// <summary>
		///     Converts the content to uppercase.
		/// </summary>
		/// <remarks>
		///     This element is defined by the AIML 1.1 specification.
		/// </remarks>
		/// <seealso cref="Formal"/><seealso cref="Lowercase"/><seealso cref="Sentence"/>
		public sealed class Uppercase : RecursiveTemplateTag {
			public Uppercase(TemplateElementCollection children) : base(children) { }

			public override string Evaluate(RequestProcess process) {
				return (this.Children?.Evaluate(process) ?? "").ToUpper();
			}

			public static TemplateNode.Uppercase FromXml(XmlNode node, AimlLoader loader) {
				return new Uppercase(TemplateElementCollection.FromXml(node, loader));
			}
		}
	}
}
