using System;
using System.IO;
using System.Xml;
using Xunit;

namespace Aiml.NET.Tests
{
    public class DefaultBasicTests
    {
        private static string WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "defaults");
        private readonly Bot _bot;
        private readonly User _botUser;

        public DefaultBasicTests()
        {
            //create bot
            this._bot = new Bot(WorkingDirectory);
            //create user
            this._botUser = new User("Test-User", this._bot);
            //load default configs
            this._bot.LoadConfig();
        }

        [Fact]
        public void Should_Load_Aiml_From_Directory()
        {
            //there is a default helloworld.aiml
            this._bot.LoadAIML();
            //Assert Size > 1 (at list one file)
            Assert.True(_bot.Size > 0);
            //test response
            string messageToSend = "hello";
            Response botResponse = _bot.Chat(new Request(messageToSend, this._botUser, _bot), false);

            Assert.False(string.IsNullOrEmpty(botResponse.ToString()));
        }

        [Fact]
        public void Should_Load_Aiml_From_Content()
        {
            //create simple AIML 
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(@"
            <aiml version='2.0'>
                <category>
                    <pattern>HI</pattern>
                    <template>
                        Hi too
                    </template>
                </category>
            </aiml>
            ");

            this._bot.LoadAIML(xmlDocument, "test-xaml-document");

            //test response
            Response botResponse = _bot.Chat(new Request("hi", this._botUser, _bot), false);

            Assert.False(string.IsNullOrEmpty(botResponse.ToString()));
            Assert.Contains("Hi too", botResponse.ToString());
        }
    }
}
