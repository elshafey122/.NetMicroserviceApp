using Mongo.ShoppingCartApi.Model;
using Mongo.ShoppingCartApi.Model.Dto;
using Mongo.ShoppingCartApi.Services.Interfaces;
using Mongo.ShoppingCartApi.Utility;
using Newtonsoft.Json;
using RestSharp;
namespace Mongo.ShoppingCartApi.Services.Implemintations
{
    public class RestRepository : IRestRepository
    {
        private readonly RestClient _restClient;
        public RestRepository()
        {
            _restClient = new RestClient();
        }

        public async Task<CouponDto> GetCouponAsync(string couponCode)
        {
            var request = new RestRequest($"{SD.CouponApi}/GetByCode/{couponCode}", Method.Get);
            var result = await _restClient.ExecuteGetAsync(request);
            var response = JsonConvert.DeserializeObject<ResponseDto>(result.Content);
            if (!response.IsSucceeded)
            {
                return new CouponDto();
            }
            CouponDto couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Data));
            return couponDto;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var request = new RestRequest($"{SD.ProductApi}/GetAll", Method.Get);
            var result = await _restClient.ExecuteGetAsync(request);
            var response = JsonConvert.DeserializeObject<ResponseDto>(result.Content);
            if (!response.IsSucceeded)
            {
                return new List<ProductDto>();
            }
            IEnumerable<ProductDto> productDtos =  JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(response.Data));
            return productDtos;
        }
    }
}
