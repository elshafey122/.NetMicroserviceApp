using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mongo.ProductApi.Services.Interfaces;
using Mongo.ShoppingCartApi.Model.Dto;

namespace Mongo.ShoppingCartApi.Controllers
{
    [Route("api/ShoppingCart")]
    [ApiController]
    [Authorize]
    public class ShoppingCartApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartApiController(IMapper mapper, IShoppingCartRepository shoppingCartRepository )
        {
            _mapper = mapper;
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpPost("CartUpsert")]
        public async Task<IActionResult> CartUpsert([FromBody] CartDto cardDto)
        {
            var result =await _shoppingCartRepository.Upsert(cardDto);
            return Ok(result);
        }

        [HttpPut("RemoveCart/{cartDetailsId}")]
        public async Task<IActionResult> RemoveCart(int cartDetailsId)
        {
            var result = await _shoppingCartRepository.RemoveCart(cartDetailsId);
            return Ok(result);
        }

        [HttpGet("GetCart/{UserId}")]
        public async Task<IActionResult> GetCart(string UserId)
        {
            var result = await _shoppingCartRepository.GetCart(UserId);
            return Ok(result);
        }

        [HttpPost("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] CartDto cardDto)
        {
            var result = await _shoppingCartRepository.ApplyCoupon(cardDto);
            return Ok(result);
        }

        [HttpPost("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon([FromBody] CartDto cardDto)
        {
            var result = await _shoppingCartRepository.RemoveCoupon(cardDto);
            return Ok(result);
        }
    }
}
