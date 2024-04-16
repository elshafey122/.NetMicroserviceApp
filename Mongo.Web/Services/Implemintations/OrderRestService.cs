using Mongo.Model.Cart.Dto;
using Mongo.Web.Model;
using Mongo.Web.Models;
using Mongo.Web.Services.Interfaces;
using Mongo.Web.Utilites;
using Newtonsoft.Json;
using RestSharp;

namespace Mongo.Web.Services.Implemintations
{
	public class OrderRestService : IOrderRestService
	{
		private readonly ITokenProvider _tokenProvider;
		private readonly RestClient _restClient;
		private readonly ResponseDto _response;
        public OrderRestService(ITokenProvider tokenProvider)
        {
			_tokenProvider = tokenProvider;
			_restClient = new RestClient();
			_response = new ResponseDto();
        }
        public async Task<ResponseDto?> CreateOrder(string url, CartDto cartDto, bool withbearer = true)
		{
			var request = new RestRequest(url, Method.Post);
			request.AddJsonBody(cartDto);
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
			else 
			{
			    _response.Data = result.Content;
                _response.IsSucceeded = true;
            }
            return _response;
		}

		public async Task<ResponseDto?> CreateStripeSession(string url, StripeRequestDto stripeRequestDto, bool withbearer = true)
		{
			var request = new RestRequest(url, Method.Post);
			request.AddJsonBody(stripeRequestDto);
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

		public async Task<ResponseDto?> GetAllOrder(string url, bool withbearer = true)
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

		public async Task<ResponseDto?> GetOrder(string url, bool withbearer = true)
		{
			var request = new RestRequest(url, Method.Get);
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
			_response.Data = result;
			return _response;
		}

		public async Task<ResponseDto?> UpdateOrderStatus(string url, string newStatus, bool withbearer = true)
		{
			var request = new RestRequest(url, Method.Post);
			request.AddJsonBody(newStatus);
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

		public async Task<ResponseDto?> ValidateStripeSession(string url, bool withbearer = true)
		{
			var request = new RestRequest(url, Method.Get);
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
			_response.Data = result;
			return _response;
		}
	}
}
