using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Logger;

namespace EfCoreInAction.Controllers
{
    public abstract class BaseTraceController : Controller
    {
        protected void SetupTraceInfo()
        {
            string traceIdentifier = HttpContext.TraceIdentifier;

            ViewData["TraceIdent"] = traceIdentifier;
            ViewData["NumLogs"] = HttpRequestLog.GetHttpRequestLog(traceIdentifier).RequestLogs.Count;
        }
    }
}
