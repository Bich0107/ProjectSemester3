using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.MiddleWares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticalMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if (path.HasValue && !path.Value.Equals("/account")
                && !path.Value.Equals("/account/login"))
            {
                if (httpContext.Session.GetString("username") == null)
                {
                    httpContext.Response.Redirect("/account/login");
                }
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticalMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticalMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticalMiddleware>();
        }
    }
}
