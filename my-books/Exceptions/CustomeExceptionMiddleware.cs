using my_books.Data.ViewModels;
using System.Net;

namespace my_books.Exceptions
{
    public class CustomeExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomeExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task OnvokeAsync(HttpContext httpContext)
        {
            try {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExcrptionAsync(httpContext, ex);
            }
        }

        private Task HandleExcrptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            var response = new ErrorVM()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal Server from the cistom middleware",
                path = "path-goes-here"
            };
            return httpContext.Response.WriteAsync(response.ToString());
        }


    }
}
