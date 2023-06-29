using System;
using System.Collections.Generic;
using System.IO;

namespace Aiml.NET.XChatBot
{
    internal class Program
    {
        /*
        *  THIS IS A SIMPLE CHAT BOT TO PLAY AROUND WITH IT
        */
        static void Main(string[] args)
        {
            try
            {
                string botPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                //create a bot
                Bot bot = new Bot(botPath);
                bot.LogMessage += OnBotLogMessageReceived;

                bot.LoadConfig();
                bot.LoadAIML();
                //get name of bot from Config
                string botName = bot.Properties.GetValueOrDefault("name", "XChatBot");
                //create a user to interact with Bot
                User user = new User("User", bot);
                while (true)
                {
                    Console.Write("> ");
                    var message = Console.ReadLine();
                    var trace = false;
                    if (message.StartsWith(".trace "))
                    {
                        trace = true;
                        message = message.Substring(7);
                    }
                    Response botResponse = bot.Chat(new Request(message, user, bot), trace);
                    //replace line breaks with Console Line Breaks
                    string[] lines = botResponse.ToString().Split(new[] { "\\r\\n" }, StringSplitOptions.None);
                    string responseString = string.Join(Environment.NewLine, lines);

                    Console.WriteLine($"{botName}: {responseString}");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e);
                Console.ResetColor();
            }
        }

        private static void OnBotLogMessageReceived(object sender, LogMessageEventArgs e)
        {
            switch (e.Level)
            {
                case LogLevel.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogLevel.Gossip: Console.ForegroundColor = ConsoleColor.Blue; break;
                case LogLevel.Chat: Console.ForegroundColor = ConsoleColor.Blue; break;
                case LogLevel.Diagnostic: Console.ForegroundColor = ConsoleColor.DarkBlue; break;
            }
            Console.WriteLine($"[{e.Level}] {e.Message}");
            Console.ResetColor();
        }
    }
}
