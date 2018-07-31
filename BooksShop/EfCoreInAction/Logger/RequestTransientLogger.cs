using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServiceLayer.Logger;
using System;

namespace EfCoreInAction.Logger
{
    public class RequestTransientLogger : ILoggerProvider
    {
        private readonly Func<IHttpContextAccessor> httpAccessor;

        public static LogLevel LogThisAndAbove { get; set; } = LogLevel.Information;

        public RequestTransientLogger(Func<IHttpContextAccessor> httpAccessor)
        {
            this.httpAccessor = httpAccessor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger(httpAccessor);
        }

        public void Dispose()
        {
        }

        private class MyLogger : ILogger
        {
            private readonly Func<IHttpContextAccessor> httpAccessor;

            public MyLogger(Func<IHttpContextAccessor> httpAccessor)
            {
                this.httpAccessor = httpAccessor;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= LogThisAndAbove;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var httpCxt = httpAccessor().HttpContext;
                if (httpCxt == null) return; //we ignore any logs that happen outside a HttpRequest

                string eventString = formatter(state, exception);
                HttpRequestLog.AddLog(httpCxt.TraceIdentifier, logLevel, eventId, eventString);
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
