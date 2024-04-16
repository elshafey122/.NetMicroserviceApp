using Mongo.Model.Cart.Dto;
using Mongo.Web.Model;

namespace Mongo.Web.Services.Interfaces
{
	public interface IOrderRestService
	{
		Task<ResponseDto?> CreateOrder(string url, CartDto cartDto, bool withbearer = true);
		Task<ResponseDto?> CreateStripeSession(string url,StripeRequestDto stripeRequestDto, bool withbearer = true);
		Task<ResponseDto?> ValidateStripeSession(string url, bool withbearer = true);
		Task<ResponseDto?> GetAllOrder(string url, bool withbearer = true);
		Task<ResponseDto?> GetOrder(string url, bool withbearer = true);
		Task<ResponseDto?> UpdateOrderStatus(string url, string newStatus, bool withbearer = true);
	}
}
