using Mongo.OrderApi.Dto;
using Mongo.OrderApi.Utility;

namespace Mongo.OrderApi.Service
{
    public interface IOrderService
    {
        Task<ResponseDto?> CreateOrder(CartDto cartDto);
        ResponseDto CreateStripeSession(StripeRequestDto stripeRequestDto);
        ResponseDto ValidateStripeSession(int orderHeaderId);
        ResponseDto GetAllOrder(string? userId,string role=SD.RoleCustomer);
        ResponseDto GetOrder(int orderId);
        ResponseDto UpdateOrderStatus(int orderId, string newStatus);
    }
}
