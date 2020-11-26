using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Chronicle.AspNetCore.AsyncApi.Middleware
{
    public class AsyncApiMiddleware
    {
        public Task Invoke(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
