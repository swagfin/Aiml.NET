using System;
using System.Xml;

namespace Aiml.NET
{
    public partial class TemplateNode
    {
        /// <summary>
        ///     Processes the content as AIML and adds it to the bot's brain, temporarily and for the current user only, and thus temporary.
        /// </summary>
        /// <remarks>
        ///     <para>Unlike other elements with content, the content of this element is not normally evaluated.
        ///			However, the special child element <c>eval</c> is replaced with the result of evaluating its own content.</para>
        ///     <para>This element is defined by the AIML 2.0 specification.</para>
        /// </remarks>
        /// <seealso cref="AddTriple"/><seealso cref="LearnF"/><seealso cref="Set"/>
        public sealed class Learn : TemplateNode
        {
            public XmlNode Node { get; }

            public Learn(XmlNode node)
            {
                this.Node = node;
            }

            public override string Evaluate(RequestProcess process)
            {
                // Evaluate <eval> tags.
                XmlNode node = this.Node.Clone();
                this.ProcessXml(node, process);

                // Check if topic exist and check it's Name attribute, Assign if Not specified
                XmlNode topicNode = node.SelectSingleNode("/topic");
                if (topicNode != null && topicNode.Attributes["name"] == null)
                {
                    XmlAttribute nameAttribute = topicNode.OwnerDocument.CreateAttribute("name");
                    nameAttribute.Value = process?.User?.Topic ?? "*";
                    topicNode.Attributes.Append(nameAttribute);
                }
                // Learn
                process.Log(LogLevel.Diagnostic, $"In element <learn>: learning new category for {process.User.ID}: {node.OuterXml}");
                AimlLoader loader = new AimlLoader(process.Bot);
                loader.LoadAIML(process.User.Graphmaster, node, null);

                return string.Empty;
            }

            public static Learn FromXml(XmlNode node, AimlLoader loader)
            {
                return new Learn(node);
            }

            private void ProcessXml(XmlNode node, RequestProcess process)
            {
                for (int i = 0; i < node.ChildNodes.Count; ++i)
                {
                    XmlNode node2 = node.ChildNodes[i];
                    if (node2.NodeType == XmlNodeType.Element)
                    {
                        if (node2.Name.Equals("eval", StringComparison.InvariantCultureIgnoreCase))
                        {
                            TemplateElementCollection tags = TemplateElementCollection.FromXml(node2, process.Bot.AimlLoader);
                            node2.ParentNode.ReplaceChild(node.OwnerDocument.CreateTextNode(tags.Evaluate(process)), node2);
                        }
                        else
                            this.ProcessXml(node2, process);
                    }
                }
            }
        }
    }
}
