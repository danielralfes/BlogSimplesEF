using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogSimplesEF.Web
{
    public class UnhandledExceptionLoggerFilter : IExceptionFilter
    {
        ILogger Log { get; } = ApplicationLogging.CreateLogger<UnhandledExceptionLoggerFilter>();

        public void OnException(ExceptionContext context)
        {
            var user = context.HttpContext?.User;
            var url = context.HttpContext?.Request.Path.ToString();
            var method = context.HttpContext?.Request.Method;

            Log.LogError(context.Exception, "[{0}] Unhandled Error on Website API - [{1} {2}]", user, method, url);
        }
    }
}
