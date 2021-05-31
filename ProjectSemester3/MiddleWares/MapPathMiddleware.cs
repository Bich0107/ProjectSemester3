using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.MiddleWares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public  class MapPathMiddleware
    {
        private readonly RequestDelegate _next;
        public MapPathMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if (httpContext.Session.GetString("username") != null 
                && httpContext.Session.GetInt32("position") == 1
                && path.HasValue
                && path.Value.StartsWith("/admin"))
            {
                    Task.Run(
                      () =>
                      {
                          string html = "<h1>No webpage was found for the web address</h1>";
                          httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                          httpContext.Response.WriteAsync(html);
                      }
                    );
            }
            if (httpContext.Session.GetString("username") != null
                && httpContext.Session.GetInt32("position") == 2
                && path.HasValue
                && path.Value.StartsWith("/superAdmin"))
            {
                                      
                      httpContext.Response.Redirect("/admin/home/index");
                 
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MapPathMiddlewareExtensions
    {
        public static IApplicationBuilder UseMapPathMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MapPathMiddleware>();
        }
    }
}
