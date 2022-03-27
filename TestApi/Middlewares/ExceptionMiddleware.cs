using Newtonsoft.Json;
using System.Net;

namespace TestApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //Logic to Log exception details for debugging the bug
            /*
             * Logic 
             */
            var response = context.Response;

            int statusCode = (int)HttpStatusCode.InternalServerError;
            //Hide crash details from end user
            string defaultMessage = "Something went wrong. Please contact business administrator";
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(
                new
                {
                    Response = statusCode == (int)HttpStatusCode.InternalServerError ? defaultMessage : (exception.GetBaseException()?.Message ?? defaultMessage),
                }));

        }
    }

}
