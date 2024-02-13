using Mongo.AuthApi.Model;
using Mongo.AuthApi.Model.Dto;

namespace Mongo.AuthApi.Services.Interfaces
{
    public interface IAuthRepository
    {
        Task<Response> RegisterAsync(RegisterationRequest registerRequest);
        Task<LoginResponse> LoginAsync(LoginRequests loginRequest);
        Task<Response> AssignRole(string Email, string role);
    }
}
