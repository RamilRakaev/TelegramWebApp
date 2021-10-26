using System.Collections.Generic;

namespace Domain.Model
{
    public class WebAppOptions
    {
        public string ApiKey
        {
            get { return Properties["ApiKey"]; }
            set { Properties["ApiKey"] = value; }
        }

        public string CalendarId
        {
            get { return Properties["CalendarId"]; }
            set { Properties["CalendarId"] = value; }
        }

        public string BotToken
        {
            get { return Properties["BotToken"]; }
            set { Properties["BotToken"] = value; }
        }

        public string HostAddress
        {
            get { return Properties["HostAddress"]; }
            set { Properties["HostAddress"] = value; }
        }

        public string this[string key]
        {
            get { return Properties[key]; }
            set { Properties[key] = value; }
        }

        public Dictionary<string, string> Properties { get; private set; } = new()
        {
            { "ApiKey", "" },
            { "CalendarId", "" },
            { "BotToken", "" },
            { "HostAddress", "" },
        };
    }
}
