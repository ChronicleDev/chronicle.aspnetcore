using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Chronicle.AspNetCore.AsyncApi.Helpers.Interfaces
{
    public interface IRequestHelper
    {
        bool RequestingAsyncApiDocument(HttpRequest request);

        bool RequestingDocumentAsYaml(HttpRequest request);
    }
}
