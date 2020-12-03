using System;
using System.Collections.Generic;
using System.Text;

namespace Chronicle.AspNetCore.AsyncApi.Options.Interfaces
{
    public interface IAsyncApiOptions
    {
        string RouteTemplate { get; }
    }
}
