using System;
using System.Collections.Generic;
using System.Text;
using Chronicle.AspNetCore.AsyncApi.Models.Interfaces;

namespace Chronicle.AspNetCore.AsyncApi.Serialization.Interfaces
{
    public interface IAsyncApiSerializer
    {
        string Serialize(IAsyncApiDocument document, bool isYaml);
    }
}
