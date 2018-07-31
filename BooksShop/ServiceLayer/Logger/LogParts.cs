using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ServiceLayer.Logger
{
    public class LogParts
    {
        private const string EFEventIdStartWith = "Microsoft.EntityFrameworkCore";

        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel LogLevel { get; set; }

        public EventId EventId { get; }

        public string EventString { get; }

        public bool IsDb => EventId.Name?.StartsWith(EFEventIdStartWith) ?? false;

        public LogParts(LogLevel logLevel, EventId eventId, string eventString)
        {
            LogLevel = logLevel;
            EventId = eventId;
            EventString = eventString;
        }

        public override string ToString()
        {
            return $"{LogLevel}: {EventString}";
        }
    }
}
