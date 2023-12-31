using System;
using System.Collections.Generic;

namespace Aiml.NET
{
    public class User
    {
        public string ID { get; }
        public Bot Bot { get; }
        public History<Request> Requests { get; }
        public History<Response> Responses { get; }
        public Dictionary<string, string> Predicates { get; }
        public PatternNode Graphmaster { get; }

        public string Topic
        {
            get => this.GetPredicate("topic");
            set { this.Predicates["topic"] = value; }
        }

        public User(string ID, Bot bot)
        {
            if (string.IsNullOrEmpty(ID)) throw new ArgumentException("The user ID cannot be empty", "ID");
            this.ID = ID;
            this.Bot = bot;
            this.Requests = new History<Request>(bot.Config.HistorySize);
            this.Responses = new History<Response>(bot.Config.HistorySize);
            this.Predicates = new Dictionary<string, string>(StringComparer.Create(bot.Config.Locale, true));
            this.Graphmaster = new PatternNode(null, bot.Config.StringComparer);
        }

        /// <summary>Returns the last sentence output from the bot to this user.</summary>
        public string GetThat() => this.GetThat(1, 1);
        /// <summary>Returns the last sentence in the <paramref name='n'/>th last message from the bot to this user.</summary>
        public string GetThat(int n) => this.GetThat(n, 1);
        /// <summary>Returns the <paramref name='n'/>th last sentence in the <paramref name='n'/>th last message from the bot to this user.</summary>
        public string GetThat(int n, int sentence)
        {
            if (n < 1 || n > this.Responses.Count)
                return this.Bot.Config.DefaultHistory;
            if (sentence < 1 || sentence > this.Responses[n - 1].Sentences.Count)
                return this.Bot.Config.DefaultHistory;
            return this.Responses[n - 1].GetLastSentence(sentence);
        }

        public string GetInput() => this.GetInput(1, 1);
        public string GetInput(int n) => this.GetInput(n, 1);
        public string GetInput(int n, int sentence)
        {
            if (n < 1 || n > this.Requests.Count)
                return this.Bot.Config.DefaultHistory;
            if (sentence < 1 || sentence > this.Requests[n - 1].Sentences.Count)
                return this.Bot.Config.DefaultHistory;
            return this.Requests[n - 1].GetLastSentence(sentence).Text;
        }

        public string GetRequest() => this.GetRequest(1);
        public string GetRequest(int n)
        {
            // Unlike <input>, the <request> tag does not count the request currently being processed.
            if (n >= 1 & n <= this.Requests.Count) return this.Requests[n - 2].Text;
            return this.Bot.Config.DefaultHistory;
        }

        public string GetResponse() => this.GetResponse(1);
        public string GetResponse(int n)
        {
            if (n >= 1 & n <= this.Responses.Count) return this.Responses[n - 1].ToString();
            return this.Bot.Config.DefaultHistory;
        }

        public void AddResponse(Response response)
        {
            this.Responses.Add(response);
        }
        public void AddRequest(Request request)
        {
            this.Requests.Add(request);
        }

        public string GetPredicate(string key)
        {
            string value;
            if (this.Predicates.TryGetValue(key, out value)) return value;
            if (this.Bot.Config.DefaultPredicates.TryGetValue(key, out value)) return value;
            return this.Bot.Config.DefaultPredicate;
        }
    }
}
