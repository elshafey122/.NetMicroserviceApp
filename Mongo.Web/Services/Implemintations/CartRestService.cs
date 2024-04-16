using Microsoft.AspNetCore.Cors.Infrastructure;
using Mongo.Model.Cart.Dto;
using Mongo.Web.Model;
using Mongo.Web.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace Mongo.Web.Services.Implemintations
{
    public class CartRestService : ICartRestService
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly RestClient _restClient;
        private readonly ResponseDto _response;
        public CartRestService(ITokenProvider tokenProvider)
        {

            _restClient = new RestClient();
            _response = new ResponseDto();
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto> ApplyCouponAsync(string url , CartDto cartDto , bool withbearer = true)
        {
            var request = new RestRequest(url, Method.Post);
            request.AddJsonBody(cartDto);
            request.AddHeader("Accept", "application/json");  //https://localhost:7003/api/ShoppingCart/ApplyCoupon
            if (withbearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}");// add token in request
            }
            var result = await _restClient.ExecuteAsync(request); //https://localhost:7003/api/ShoppingCart/ApplyCoupon
            if (result.IsSuccessful == false)
            {
                _response.IsSucceeded = false;
                _response.Message = result.StatusCode.ToString();//"https://localhost:7003/api/ShoppingCart/ApplyCoupon"
            }
            return _response;

        }

        public async Task<ResponseDto> EmailCart(string url , CartDto cartDto , bool withbearer = true)
        {
            var request = new RestRequest(url, Method.Post);
            request.AddJsonBody(cartDto);
            request.AddHeader("Accept", "application/json"); 
            if (withbearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}");// add token in request
            }
            var result = await _restClient.ExecuteAsync(request); 
            ResponseDto responseDto = JsonConvert.DeserializeObject<ResponseDto>(result.Content);
            if (responseDto.IsSucceeded == false)
            {
                _response.IsSucceeded = false;
                _response.Message = result.StatusCode.ToString();
            }
            return _response;
        }

        public async Task<ResponseDto> GetCartByUserIdAsync(string url, bool withbearer = true)
        {
            var request = new RestRequest(url,Method.Get);
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
            return JsonConvert.DeserializeObject<ResponseDto>(Convert.ToString(result.Content));
        }

        public async Task<ResponseDto> RemoveFromCartAsync(string url, bool withbearer = true)
        {
            var request = new RestRequest(url,Method.Put);
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

        public async Task<ResponseDto> UpsertCartAsync(string url , CartDto cartDto , bool withbearer = true)
        {
            var request = new RestRequest(url,Method.Post);
            request.AddJsonBody(cartDto);
            request.AddHeader("Accept", "application/json");
            if (withbearer)
            {
                request.AddHeader("Authorization", $"Bearer {_tokenProvider.GetToken()}");// add token in request
            }
            var result = await _restClient.ExecuteAsync(request); // https://localhost:7003/api/ShoppingCart/CartUpsert

            if (result.IsSuccessful == false)
            {
                _response.IsSucceeded = false;
                _response.Message = result.StatusCode.ToString();
            }
            return _response;
        }
    }
}
