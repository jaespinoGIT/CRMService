using System;
using System.Diagnostics;
using System.Web;
using Serilog;
using Serilog.Events;

namespace CRMService.Helpers.Loggers
{
    public static class Logger
    {
        private static readonly ILogger _errorLogger;

        static Logger()
        {
            _errorLogger = new LoggerConfiguration()
                .WriteTo.File(HttpContext.Current.Server.MapPath("~/logs/log-.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void LogError(string error)
        {
            _errorLogger.Error(error);
        }
    }
}
