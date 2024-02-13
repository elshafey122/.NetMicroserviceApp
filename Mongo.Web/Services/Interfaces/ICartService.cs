using Mongo.Model.Cart.Dto;
using Mongo.Web.Model;

namespace Mongo.Web.Services.Interfaces
{
    public interface IcartService
    {
        Task<ResponseDto> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto> RemoveFromCartAsync(int cartdetailsId);
        Task<ResponseDto> GetCartByUserIdAsync(int userId);
        Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
    }
}
