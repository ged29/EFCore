namespace ServiceLayer.Logger
{
    public class TraceIdentBaseDto
    {
        public string TraceIdentifier { get; private set; }

        public int LogsCount { get; private set; }

        public TraceIdentBaseDto(string traceIdentifier)
        {
            TraceIdentifier = traceIdentifier;
            LogsCount = HttpRequestLog.GetHttpRequestLog(traceIdentifier).RequestLogs.Count;
        }
    }
}