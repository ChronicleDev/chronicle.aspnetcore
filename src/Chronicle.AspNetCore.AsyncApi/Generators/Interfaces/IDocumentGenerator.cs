using System;
using System.Collections.Generic;
using System.Text;
using Chronicle.AspNetCore.AsyncApi.Models.Interfaces;

namespace Chronicle.AspNetCore.AsyncApi.Generators.Interfaces
{
    public interface IDocumentGenerator
    {
        IAsyncApiDocument GetAsyncApi();
    }
}
