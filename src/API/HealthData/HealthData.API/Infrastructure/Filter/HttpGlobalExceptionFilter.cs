namespace HealthData.API.Infrastructure.Filter;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<HttpGlobalExceptionFilter> _logger;
    private readonly IWebHostEnvironment _env;
    public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
    {
        _logger = logger;
        _env = env;
    }
    public void OnException(ExceptionContext context)
    {
        _logger.LogError(new EventId(context.Exception.HResult),
           context.Exception,
           context.Exception.Message);
        var jsonResponse = new JsonErrorResponse
        {
            Messages = new[] { "An error occured. Try it again." }
        };
        context.Result = new InternalServerErrorObjectResult(jsonResponse);
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.ExceptionHandled = true;
    }
} 

