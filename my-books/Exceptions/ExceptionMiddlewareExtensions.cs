using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using my_books.Data.ViewModels;
using System.Net;

namespace my_books.Exceptions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureBuildinExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var contextRequest = context.Features.Get<IHttpRequestFeature>();
                    if (contextFeature != null)
                    {
                         await context.Response.WriteAsync(new ErrorVM()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message =contextFeature.Error.Message,
                                path = contextRequest.Path
                            }.ToString());
                    }
                });
            });
        }
        public static void ConfigureCustomeExceptionHandle(this IApplicationBuilder app)
        { 
            app.UseMiddleware<CustomeExceptionMiddleware>();
        }
    }
}
