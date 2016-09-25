using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Waes.Diffly.Api.Infrastructure
{
    /// <summary>
    /// Logs all unhandled exceptions.
    /// </summary>
    public class GlobalLoggingExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalLoggingExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("GlobalLoggingExceptionFilter.");
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogInformation("ERROR" + context.Exception);
        }
    }
}
