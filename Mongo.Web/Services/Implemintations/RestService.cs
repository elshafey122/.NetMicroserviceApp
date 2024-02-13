using Mongo.Web.Model;
using Mongo.Web.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System.Text.Json.Serialization;

namespace Mongo.Web.Services.Implemintations
{
    public class RestService<T> : IRestService<T> where T : class
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly RestClient _restClient;
        private readonly ResponseDto _response;
        public RestService(ITokenProvider tokenProvider)
        {

            _restClient = new RestClient();
            _response = new ResponseDto();
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto> CreateAsync(string url, T data, bool withbearer = true)
        {
            var request = new RestRequest(url, Method.Post);
            request.AddJsonBody(data);
            request.AddHeader("Accept", "application/json");
            if (withbearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}");// add token in request
            }
            var result = await _restClient.ExecuteAsync(request);
            if (result.IsSuccessful == false)
            {
                _response.IsSucceeded = false;
                _response.Message = result.StatusCode.ToString();
            }
            return _response;
        }

        public async Task<ResponseDto> DeleteAsync(string url, bool withbearer = true)
        {
            var request = new RestRequest(url, Method.Delete);
            if (withbearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}");
            }
            var result = await _restClient.ExecuteAsync(request);
            if (result.IsSuccessful == false)
            {
                _response.IsSucceeded = false;
                _response.Message = result.StatusCode.ToString();
            }
            return _response;
        }

        public async Task<ResponseDto> GetAsync(string url ,bool withbearer = true)
        {
            var request = new RestRequest(url, Method.Get);
            if (withbearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}");
            }
            var result = await _restClient.ExecuteGetAsync(request);
            if (result.IsSuccessful == false)
            {
               _response.Message = result.StatusCode.ToString();
               _response.IsSucceeded = false;
               return _response;
            }
            var response = JsonConvert.DeserializeObject<ResponseDto>(result.Content);
            return response;
        }

        public async Task<ResponseDto> GetByIdAsync(string url, bool withbearer = true)
        {
            var request = new RestRequest(url, Method.Get);
            if (withbearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}");
            }
            var result = await _restClient.ExecuteAsync<List<T>>(request);
            if (result.IsSuccessful == false)
            {
                _response.IsSucceeded = false;
                _response.Message = result.StatusCode.ToString();
            }
            _response.Data = result.Data;
            return _response;
        }

        public async Task<ResponseDto> UpdateAsync(string url, T data, bool withbearer = true)
        {
            var request = new RestRequest(url, Method.Put);
            request.AddJsonBody(data);
            request.AddHeader("Accept", "application/json");
            if (withbearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}");
            }
            var result = await _restClient.ExecuteAsync(request);
            if (result.IsSuccessful == false)
            {
                _response.IsSucceeded = false;
                _response.Message = result.StatusCode.ToString();
            }
            return _response;
        }
    }
}