using Mongo.Model.Cart.Dto;
using Mongo.Web.Model;
using Mongo.Web.Services.Interfaces;

namespace Mongo.Web.Services.Implemintations
{
    public class cartService : IcartService
    {
        public Task<ResponseDto> ApplyCouponAsync(CartDto cartDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetCartByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> RemoveFromCartAsync(int cartdetailsId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpsertCartAsync(CartDto cartDto)
        {
            throw new NotImplementedException();
        }
    }
}
