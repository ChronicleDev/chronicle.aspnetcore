using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Chronicle.AspNetCore.AsyncApi.Helpers.Interfaces;
using Chronicle.AspNetCore.AsyncApi.Options.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Chronicle.AspNetCore.AsyncApi.Helpers
{
    public class RequestHelper : IRequestHelper
    {
        private readonly TemplateMatcher requestMatcher;

        public RequestHelper(IAsyncApiOptions options)
        {
            this.requestMatcher = new TemplateMatcher(TemplateParser.Parse(options.RouteTemplate), new RouteValueDictionary());
        }

        public bool RequestingAsyncApiDocument(HttpRequest request)
        {
            if (!HttpMethods.IsGet(request.Method))
            {
                return false;
            }

            var routeValues = new RouteValueDictionary();

            var result = this.requestMatcher.TryMatch(request.Path, routeValues);

            return result;
        }

        public bool RequestingDocumentAsYaml(HttpRequest request)
        {
            return string.Equals(Path.GetExtension(request.Path.Value), ".yaml", StringComparison.OrdinalIgnoreCase);
        }
    }
}
