using Mongo.Web.Model;
using Mongo.Web.Model.Auth;

namespace Mongo.Web.Services.Interfaces
{
    public interface IAuthRestService
    {
        Task<ResponseDto> LoginAsync(string url ,LoginRequests loginRequests);
        Task<ResponseDto> RegisterAsync(string url ,RegisterationRequest registerationRequest);
        Task<ResponseDto> AssignRoleAsync(string url, string role);
    }
}
