using Mongo.ShoppingCartApi.Model;
using Mongo.ShoppingCartApi.Model.Dto;

namespace Mongo.ShoppingCartApi.Services.Interfaces
{
    public interface IRestRepository
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<CouponDto> GetCouponAsync(string couponCode);
    }
}
