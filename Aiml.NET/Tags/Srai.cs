using System.Xml;

namespace Aiml.NET
{
    public partial class TemplateNode
    {
        /// <summary>
        ///     Recurses the content into a new request and returns the result.
        /// </summary>
        /// <remarks>
        ///     <para>The content is evaluated and then processed as if it had been entered by the user, including normalisation and other pre-processing.</para>
        ///     <para>It is unknown what 'sr' stands for, but it's probably 'symbolic reduction'.</para>
        ///     <para>This element is defined by the AIML 1.1 specification.</para>
        /// </remarks>
        /// <seealso cref="Sr"/><seealso cref="SraiX"/>
        public sealed class Srai : RecursiveTemplateTag
        {
            public Srai(TemplateElementCollection children) : base(children) { }

            public override string Evaluate(RequestProcess process)
            {
                string text = this.Children?.Evaluate(process) ?? "";
                text = string.IsNullOrEmpty(text) ? "unknown" : text;
                process.Log(LogLevel.Diagnostic, "In element <srai>: processing text '" + text + "'.");
                var newRequest = new Aiml.NET.Request(text, process.User, process.Bot);
                text = process.Bot.ProcessRequest(newRequest, false, false, process.RecursionDepth + 1, out _).ToString().Trim();
                process.Log(LogLevel.Diagnostic, "In element <srai>: the request returned '" + text + "'.");
                return text;
            }

            public static Srai FromXml(XmlNode node, AimlLoader loader)
            {
                return new Srai(TemplateElementCollection.FromXml(node, loader));
            }
        }
    }
}
