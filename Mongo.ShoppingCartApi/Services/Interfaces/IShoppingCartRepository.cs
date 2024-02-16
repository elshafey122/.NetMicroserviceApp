using Mongo.ShoppingCartApi.Model;
using Mongo.ShoppingCartApi.Model.Dto;

namespace Mongo.ProductApi.Services.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ResponseDto> Upsert(CartDto cartDto);
        Task<ResponseDto> RemoveCart(int cartdetailsId);
        Task<ResponseDto> GetCart(string userId);
        Task<ResponseDto> ApplyCoupon(CartDto cartDto);
        Task<ResponseDto> RemoveCoupon(CartDto cartDto);
        Task<ResponseDto> EmailCartRequest(CartDto cartDto , string messageName);
    }
}
