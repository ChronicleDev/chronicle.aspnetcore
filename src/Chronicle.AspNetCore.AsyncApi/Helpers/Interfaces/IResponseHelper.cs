using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Chronicle.AspNetCore.AsyncApi.Helpers.Interfaces
{
    public interface IResponseHelper
    {
        Task RespondWithDocument(HttpResponse response, bool isYaml);

        void RespondWithNotFound(HttpResponse response);
    }
}