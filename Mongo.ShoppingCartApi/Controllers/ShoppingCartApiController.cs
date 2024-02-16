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
        private readonly IConfiguration _configuration;
        public ShoppingCartApiController(IMapper mapper, IShoppingCartRepository shoppingCartRepository, IConfiguration configuration)
        {
            _mapper = mapper;
            _shoppingCartRepository = shoppingCartRepository;
            _configuration = configuration;
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

        [HttpPost("EmailCartRequest")]
        public async Task<IActionResult> EmailCartRequest([FromBody] CartDto cardDto)
        {
            string messageName = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCart");
            var result = await _shoppingCartRepository.EmailCartRequest(cardDto,messageName);
            return Ok(result);
        }
    }
}
