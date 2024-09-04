using System;
using System.Diagnostics;
using System.Web;
using Serilog;

namespace WebApi
{
    public static class SerilogLogger
    {
        public static Serilog.ILogger _logger;

        static SerilogLogger() {
			_logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.File(HttpContext.Current.Server.MapPath("~/logs.txt"))
				.ReadFrom.AppSettings()
				.Enrich.WithHttpRequestUrl()
				.Enrich.WithHttpRequestType()
				.Enrich.WithHttpRequestClientHostIP()
				.CreateLogger();
		}
    }
}