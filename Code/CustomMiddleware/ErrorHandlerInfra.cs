using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.CustomMiddleware
{
    /// <summary>
    /// The class that will well-form the error information
    /// </summary>
    public class ErrorInformation
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Global Class that will be used by Middleware
    /// to handle errors across app.
    /// 1. This class will be injected with 'RequestDelegate'
    ///     RequestDelegate, is a delegate that accepts 'HttpContext'
    ///         HttpContext represent the current request in execution
    /// 2. Must have 'InvokeAsync()' method that will contain the Middleware logic
    ///     This method will accept HttpContext and the logic object as parameters
    ///         The 'logic Object', the logic to be executed e.g. Exception
    /// </summary>
    public class AppErrorHandler
    {
        private readonly RequestDelegate _next;

        public AppErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext http)
        {
            try
            {
                // if no error while executing the request
                // then just go to next() middleware in pipeline

                await _next(http);
            }
            catch (Exception ex)
            {
                // logic to return the error using HttpResponse
                await HandleErrorAsync(http, ex);
            }
        }

        private async Task HandleErrorAsync(HttpContext ctx, Exception ex)
        {
            // set the response code
            ctx.Response.StatusCode = 500;

            // write the response in object
            var errorInfo = new ErrorInformation()
            {
                 ErrorCode = ctx.Response.StatusCode,
                 ErrorMessage = ex.Message
            };

            // write the errorInfo in the response as JSON
            var jsonResponse = JsonConvert.SerializeObject(errorInfo);

             await ctx.Response.WriteAsync(jsonResponse);

        }
    }

    /// <summary>
    /// The Custom Extension class for Registering
    /// the Middleware
    /// </summary>
    public static  class CustomErrorMiddleware
    {
        public static void UseCustomErrorHandlerMiddleware(this IApplicationBuilder app)
        {
            // load the class having RequestDelegate injected and an InvokeAsync()
            // as the parameter type for UseMiddleware() method of IApplicationBuilder
            app.UseMiddleware<AppErrorHandler>();
        }
    }
}
