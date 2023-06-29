using System.Xml;

namespace Aiml.NET
{
    public partial class TemplateNode
    {
        /// <summary>
        ///    Line Break Tag
        /// </summary>
        /// <remarks>
        ///     <para>This element has no content.</para>
        ///     <para>This element is not defined by the AIML 1.1 specification.</para>
        /// </remarks>
        public sealed class Br : TemplateNode
        {
            public override string Evaluate(RequestProcess process)
            {
                return "\n";
            }

            public static Br FromXml(XmlNode node, AimlLoader loader)
            {
                return new Br();  // The Br tag supports no properties.
            }
        }
    }
}
