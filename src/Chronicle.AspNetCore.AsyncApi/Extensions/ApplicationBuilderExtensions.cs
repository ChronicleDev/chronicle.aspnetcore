using System;
using System.Collections.Generic;
using System.Text;
using Chronicle.AspNetCore.AsyncApi.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Chronicle.AspNetCore.AsyncApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAsyncApi(this IApplicationBuilder app)
        {
            app.UseMiddleware<AsyncApiMiddleware>();

            return app;
        }
    }
}
