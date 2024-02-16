using Mongo.Model.Cart.Dto;
using Mongo.Web.Model;

namespace Mongo.Web.Services.Interfaces
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserIdAsync(string url, bool withbearer = true);
        Task<ResponseDto> UpsertCartAsync(string url, CartDto cartDto, bool withbearer = true);
        Task<ResponseDto> RemoveFromCartAsync(string url , bool withbearer = true);
        public Task<ResponseDto> ApplyCouponAsync(string url, CartDto cartDto,bool withbearer = true);
        public Task<ResponseDto> EmailCart(string url, CartDto cartDto, bool withbearer = true);

    }
}
