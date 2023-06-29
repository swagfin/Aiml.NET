using System;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Aiml.NET
{
    public partial class TemplateNode
    {
        /// <summary>
        ///    Invokes an Internal Function
        /// </summary>
        /// <remarks>
        ///     <para>A Custom Invoke</para>
        ///     <para>This element is not defined by the AIML 1.1 specification.</para>
        /// </remarks>
        public sealed class Invoke : TemplateNode
        {
            private string MethodName;
            private string MethodNamespace { get; set; } = null;
            private string MethodArgs { get; set; } = null;

            public Invoke(string methodName, string methodNamespace, string methodArgs)
            {
                this.MethodName = methodName;
                this.MethodNamespace = methodNamespace;
                this.MethodArgs = methodArgs;
            }

            public override string Evaluate(RequestProcess process)
            {

                process.Log(LogLevel.Diagnostic, "Executing <invoke>: " + MethodName + "'.");
                string invokeResponse = InvokeMethod(process.User);
                process.Log(LogLevel.Diagnostic, "In element <invoke>: the method returned '" + invokeResponse + "'.");
                return invokeResponse;
            }

            public static Invoke FromXml(XmlNode node, AimlLoader loader)
            {

                return new Invoke(
                                   node.Attributes["method"]?.Value ?? throw new Exception("<invoke> property is missing method property"),
                                   node.Attributes["namespace"]?.Value,
                                   node.Attributes["args"]?.Value
                                  );
            }
            private string InvokeMethod(User user)
            {
                try
                {
                    //check params
                    if (string.IsNullOrEmpty(MethodName)) return "method name was not provided";
                    if (string.IsNullOrEmpty(MethodNamespace))
                    {
                        MethodNamespace = Assembly.GetEntryAssembly().GetName().Name;
                    };
                    // Find in Entry Assembly
                    MethodInfo methodInfo = Assembly.GetEntryAssembly().GetTypes()
                                                                     ?.Where(t => t.Namespace == MethodNamespace.Trim()).ToList()
                                                                     ?.SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                                                                     ?.FirstOrDefault(x => x.Name == MethodName.Trim());
                    //check if method was found
                    if (methodInfo != null)
                    {
                        object[] args = string.IsNullOrEmpty(MethodArgs) ? new object[] { user.ID, user.Predicates }
                                        : (MethodArgs.Equals("null") || MethodArgs.Equals("nil")) ? null
                                        : new object[] { MethodArgs };

                        _ = methodInfo.Invoke(null, args);
                        return "success";
                    }
                    else
                    {
                        return "method: " + MethodName + " missing in namespace: " + MethodNamespace;
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }

            }
        }
    }
}
