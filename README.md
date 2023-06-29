# Aiml.NET
Aiml.NET is a .NET library for running chat robots using AIML [Artificial Intelligence Markup Language](http://www.aiml.foundation/).

## Install 

*NuGet Package*
```
Install-Package Aiml.NET
```
https://nuget.org/packages/Aiml.NET

## Usage Example

```cs
using Aiml.NET;

var bot = new Bot(botPath);
//load bot config files
bot.LoadConfig();
// load bot aiml files
bot.LoadAIML();

var user = new User("User", bot);
while (true)
{
	Console.Write("> ");
	var message = Console.ReadLine();
	var response = bot.Chat(new Request(message, user, bot), trace);
	Console.WriteLine($"{bot.Properties.GetValueOrDefault("name", "Robot")}: {response}");
}
```
