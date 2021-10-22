
using System.Collections;
using System.Collections.Generic;

namespace Domain.Model
{
    public class WebAppOptions : IEnumerable
    {
        public string ApiKey { get; set; }
        public string CalendarId { get; set; }
        public string BotToken { get; set; }
        public string HostAddress { get; set; }

        private Dictionary<string, string> properties = new Dictionary<string, string>
        {
            {"ApiKey","" },
            {"CalendarId","" },
            {"BotToken","" },
            {"HostAddress","" },
        };

        public IEnumerator GetEnumerator()
        {
            foreach(var propertyName in properties.Keys)
            {
                yield return properties[propertyName];
            }
        }
    }
}
