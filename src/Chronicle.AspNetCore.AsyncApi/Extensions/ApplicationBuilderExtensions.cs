using System;
using System.Collections.Generic;
using System.Text;
using Chronicle.AspNetCore.AsyncApi.Helpers.Interfaces;
using Chronicle.AspNetCore.AsyncApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle.AspNetCore.AsyncApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAsyncApi(this IApplicationBuilder app)
        {
            var requestHelper = app.ApplicationServices.GetRequiredService<IRequestHelper>();
            var responseHelper = app.ApplicationServices.GetRequiredService<IResponseHelper>();

            app.UseMiddleware<AsyncApiMiddleware>(requestHelper, responseHelper);

            return app;
        }
    }
}
