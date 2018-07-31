using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ServiceLayer.Logger
{
    /// <summary>
    /// This class handles the storing/retrieval of logs for each Http request, as defined by 
    /// ASP.NET Core's TraceIdentifier. 
    /// It uses a static ConcurrentDictionary to hold the logs. 
    /// NOTE: THIS WILL NOT WORK WITH SCALE OUT, i.e. it will not work if multiple instances of the web app are running
    /// </summary>
    public class HttpRequestLog
    {
        private const int MaxKeepLogMinutes = 10;

        private static readonly ConcurrentDictionary<string, HttpRequestLog> AllHttpRequestLogs = new ConcurrentDictionary<string, HttpRequestLog>();

        private readonly List<LogParts> RequestLogsInner;

        public string TraceIdentifier { get; }

        public DateTime LastAccessed { get; private set; }

        public ImmutableList<LogParts> RequestLogs => RequestLogsInner.ToImmutableList();

        private void ClearOldLogs(int maxKeepLogMinutes)
        {
            var now = DateTime.UtcNow;
            var logToRemove = AllHttpRequestLogs.Values
                .OrderBy(x => x.LastAccessed)
                .Where(x => now.Subtract(x.LastAccessed).TotalMinutes > maxKeepLogMinutes).ToList();

            logToRemove.ForEach(log => AllHttpRequestLogs.TryRemove(log.TraceIdentifier, out HttpRequestLog value));
        }

        private HttpRequestLog(string traceIdentifier)
        {
            TraceIdentifier = traceIdentifier;
            LastAccessed = DateTime.UtcNow;
            RequestLogsInner = new List<LogParts>();
            //now clear old request logs
            ClearOldLogs(MaxKeepLogMinutes);
        }

        public static void AddLog(string traceIdentifier, LogLevel logLevel, EventId eventId, string eventString)
        {
            var thisSessionLog = AllHttpRequestLogs.GetOrAdd(traceIdentifier, new HttpRequestLog(traceIdentifier));
            thisSessionLog.RequestLogsInner.Add(new LogParts(logLevel, eventId, eventString));
            thisSessionLog.LastAccessed = DateTime.UtcNow;
        }

        public static HttpRequestLog GetHttpRequestLog(string traceIdentifier)
        {
            HttpRequestLog result;
            if (AllHttpRequestLogs.TryGetValue(traceIdentifier, out result)) return result;

            //No log so make up one to say what has happened.
            var oldest = AllHttpRequestLogs.Values.OrderBy(x => x.LastAccessed).FirstOrDefault();
            result = new HttpRequestLog(traceIdentifier);
            result.RequestLogs.Add(new LogParts(
                LogLevel.Warning,
                new EventId(1, "EfCoreInAction"),
                $"Could not find the log you asked for. I have {AllHttpRequestLogs.Keys.Count} logs" + (oldest == null ? "." : $" the oldest is {oldest.LastAccessed:s}")));

            return result;
        }

        public override string ToString()
        {
            return $"At time: {LastAccessed:s}, Logs : {string.Join("/n", RequestLogsInner.Select(x => x.ToString()))}";
        }
    }
}
