using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Chronicle.AspNetCore.AsyncApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAsyncApi(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
