using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace QuizApplication.CustomMiddleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentException ex)
            {
                await HandleArgumentExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleArgumentExceptionAsync(HttpContext context, ArgumentException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var title = "Bad Request. ";
            var result = JsonConvert.SerializeObject(new
            {
                statusCode = (int)HttpStatusCode.BadRequest,
                message = title + exception.Message
            });

            System.Diagnostics.Debug.WriteLine(exception.Message);

            return context.Response.WriteAsync(result);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var title = "Internal Server Error. ";
            var result = JsonConvert.SerializeObject(new
            {
                statusCode = (int)HttpStatusCode.InternalServerError,
                message = title + exception.Message
            });

            System.Diagnostics.Debug.WriteLine(exception.Message);

            return context.Response.WriteAsync(result);
        }
    }
}
