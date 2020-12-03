using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Chronicle.AspNetCore.AsyncApi.Generators.Interfaces;
using Chronicle.AspNetCore.AsyncApi.Helpers.Interfaces;
using Chronicle.AspNetCore.AsyncApi.Serialization.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Chronicle.AspNetCore.AsyncApi.Middleware
{
    public class AsyncApiMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IRequestHelper requestHelper;
        private readonly IResponseHelper responseHelper;

        public AsyncApiMiddleware(RequestDelegate next, IRequestHelper requestHelper, IResponseHelper responseHelper)
        {
            this.next = next;
            this.requestHelper = requestHelper;
            this.responseHelper = responseHelper;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!this.requestHelper.RequestingAsyncApiDocument(context.Request))
            {
                await this.next(context);
                return;
            }

            try
            {
                var isYaml = this.requestHelper.RequestingDocumentAsYaml(context.Request);

                await this.responseHelper.RespondWithDocument(context.Response, isYaml);
            }
            catch
            {
                this.responseHelper.RespondWithNotFound(context.Response);
            }
        }
    }
}
