using Mongo.Web.Model;
using Mongo.Web.Model.Auth;
using Mongo.Web.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace Mongo.Web.Services.Implemintations
{
    public class AuthRestService : IAuthRestService
    {
        private readonly RestClient _restClient;
        private readonly ResponseDto _response;
        public AuthRestService()
        {

            _restClient = new RestClient();
            _response = new ResponseDto();
        }

        public async Task<ResponseDto> AssignRoleAsync(string url, string role)
        {
            var request = new RestRequest(url,Method.Post);
            request.AddJsonBody(role);
            request.AddHeader("Accept", "application/json");
            var result = await _restClient.ExecuteAsync(request);
            if (!result.IsSuccessful)
            {
                _response.IsSucceeded = false;
                _response.Message = result.ErrorMessage;
            }
            return _response;
        }

        public async Task<ResponseDto> LoginAsync(string url, LoginRequests loginRequests)
        {
            var request = new RestRequest(url, Method.Post);
            request.AddJsonBody(loginRequests);
            request.AddHeader("Accept", "application/json");
            var result =await _restClient.ExecuteAsync(request);
            var response = JsonConvert.DeserializeObject<ResponseDto>(result.Content);
            return response;
        }

        public async Task<ResponseDto> RegisterAsync(string url, RegisterationRequest registerationRequest)
        {
            var request = new RestRequest(url, Method.Post);
            request.AddJsonBody(registerationRequest);
            request.AddHeader("Accept", "application/json");
            var result = await _restClient.ExecuteAsync(request);
            var response = JsonConvert.DeserializeObject<ResponseDto>(result.Content);
            return response;
        }
    }
}
