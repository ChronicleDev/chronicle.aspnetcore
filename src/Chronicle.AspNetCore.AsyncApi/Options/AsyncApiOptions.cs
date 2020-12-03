using System;
using System.Collections.Generic;
using System.Text;
using Chronicle.AspNetCore.AsyncApi.Options.Interfaces;

namespace Chronicle.AspNetCore.AsyncApi.Options
{
    public class AsyncApiOptions : IAsyncApiOptions
    {
        public string RouteTemplate { get; set; } = "async-api/async-api.{json|yaml}";
    }
}
