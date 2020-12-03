using System;
using System.Text;
using System.Threading.Tasks;
using Chronicle.AspNetCore.AsyncApi.Generators.Interfaces;
using Chronicle.AspNetCore.AsyncApi.Helpers.Interfaces;
using Chronicle.AspNetCore.AsyncApi.Serialization.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Chronicle.AspNetCore.AsyncApi.Helpers
{
    public class ResponseHelper : IResponseHelper
    {
        private readonly IDocumentGenerator documentGenerator;
        private readonly IAsyncApiSerializer serializer;

        public ResponseHelper(IDocumentGenerator documentGenerator, IAsyncApiSerializer serializer)
        {
            this.documentGenerator = documentGenerator;
            this.serializer = serializer;
        }

        public async Task RespondWithDocument(HttpResponse response, bool isYaml)
        {
            response.StatusCode = 200;
            response.ContentType = isYaml ? "text/yaml;charset=utf-8" : "application/json;charset=utf-8";

            var document = this.documentGenerator.GetAsyncApi();

            var content = this.serializer.Serialize(document, isYaml);

            await response.WriteAsync(content, new UTF8Encoding(false));
        }

        public void RespondWithNotFound(HttpResponse response)
        {
            response.StatusCode = 404;
        }
    }
}