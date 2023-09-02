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

## AIML Example File
```xml
<?xml version="1.0" encoding="UTF-8"?>
<aiml version="2">
  <category>
    <pattern>HI</pattern>
    <template>
      <random>
        <li>Hello!</li>
        <li>Hi there!</li>
        <li>Greetings!</li>
      </random>
    </template>
  </category>
</aiml>

```

### Resources
 You can learn and code your own AIML Files from [PandoraBots Platform](https://home.pandorabots.com/home.html) 
 or quickly jump into;
 1. [AIML Syntax Reference](https://www.pandorabots.com/docs/aiml-reference/)
 2. [AIML ALICE 2.0 Training Files](https://code.google.com/archive/p/aiml-en-us-foundation-alice2/)
 3. [Free A.L.I.C.E. AIML Training Set](https://code.google.com/archive/p/aiml-en-us-foundation-alice/)
