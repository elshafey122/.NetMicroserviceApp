using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mongo.Model.Cart.Dto;
using Mongo.Web.Services.Interfaces;
using Mongo.Web.Utilites;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mongo.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> CartIndex()
        {
            var cartDto = new CartDto();
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
            var response = await _cartService.GetCartByUserIdAsync($"{SD.ShoppingCartApi}/GetCart/{userId}", true);
            if (response.IsSucceeded = false)
            {
                TempData["error"] = response.Message;
                return View(new CartDto());
            }
            cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Data));
            if(cartDto == null)
            {
                return View(new CartDto());
            }
            return View(cartDto);
        }

        public async Task<IActionResult> Remove(int CartDetailsId)
        {
            var response = await _cartService.RemoveFromCartAsync($"{SD.ShoppingCartApi}/RemoveCart/{CartDetailsId}", true);
            if (response.IsSucceeded)
            {
                TempData["success"] = "item deleted successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        
        public async Task<IActionResult> ApplyCoupon(CartDto cardDto)
        {
            var response = await _cartService.ApplyCouponAsync($"{SD.ShoppingCartApi}/ApplyCoupon",cardDto,true);
            if (response.IsSucceeded)
            {
                TempData["success"] = "cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        public async Task<IActionResult> RemoveCoupon(CartDto cardDto)
        {
            var response = await _cartService.ApplyCouponAsync($"{SD.ShoppingCartApi}/ApplyCoupon", cardDto, true);
            if (response.IsSucceeded)
            {
                TempData["success"] = "cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
    }
}
