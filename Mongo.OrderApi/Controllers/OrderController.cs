using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mongo.MessageBus;
using Mongo.OrderApi.Data;
using Mongo.OrderApi.Dto;
using Mongo.OrderApi.Model;
using Mongo.OrderApi.Service;
using Mongo.OrderApi.Utility;
using Stripe;
using Stripe.Checkout;
 
namespace Mongo.OrderApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        public OrderController(IOrderService orderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("GetOrders")]
        public ResponseDto? GetOrders(string? userId = "")
        {
            ResponseDto _response = new();
            if (User.IsInRole(SD.RoleAdmin))
                _response = _orderService.GetAllOrder(userId, SD.RoleAdmin);
            else
                _response = _orderService.GetAllOrder(userId);

            return _response;
        }


        //[Authorize]
        [HttpGet("GetOrder/{id:int}")]
        public ResponseDto? GetOrder(int id)
        {
            ResponseDto _response = new();
            _response = _orderService.GetOrder(id);
            return _response;
        }



        //[Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDto> CreateOrder([FromBody] CartDto cartDto)
        {
            ResponseDto _response = new();
            var result  = await _orderService.CreateOrder(cartDto);
            if (result.IsSuccess)
            {
                _response.IsSuccess = true;
                _response.Result = result.Result;
            }
            else
                _response.IsSuccess = false;

            return _response;
        }


        //[Authorize]
        [HttpPost("CreateStripeSession")]
        public  ResponseDto CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            ResponseDto _response = new();
            _response =  _orderService.CreateStripeSession(stripeRequestDto);
            return _response;
        }


        //[Authorize]
        [HttpPost("ValidateStripeSession")]
        public async Task<ResponseDto> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            ResponseDto _response = new();
            _response = _orderService.ValidateStripeSession(orderHeaderId);
            return _response;
        }


        //[Authorize]
        [HttpPost("UpdateOrderStatus/{orderId:int}")]
        public async Task<ResponseDto> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            ResponseDto _response = new();
            _response = _orderService.UpdateOrderStatus(orderId, newStatus);
            return _response;
        }
    }
}
