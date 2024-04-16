using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mongo.Model.Cart.Dto;
using Mongo.Web.Model;
using Mongo.Web.Model.Order.Dto;
using Mongo.Web.Services.Implemintations;
using Mongo.Web.Services.Interfaces;
using Mongo.Web.Utilites;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mongo.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRestService _cartService;
        private readonly IOrderRestService _orderRestService;
        public CartController(ICartRestService cartService, IOrderRestService orderRestService)
        {
            _cartService = cartService;
            _orderRestService = orderRestService;
        }

        public async Task<IActionResult> CartIndex()
        {
            ResponseDto _response = new();
            var cartDto = new CartDto();
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
            _response = await _cartService.GetCartByUserIdAsync($"{SD.ShoppingCartApi}/GetCart/{userId}", true);
            if (_response.IsSucceeded = false)
            {
                TempData["error"] = _response.Message;
                return View(new CartDto());
            }
            cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(_response.Data));
            if(cartDto == null)
            {
                return View(new CartDto());
            }
            return View(cartDto);
        }

        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            CartDto cartDto = await LoadCartDtoBasedOnLoggedInUser();
            return View(cartDto);
        }


        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            OrderHeaderDto orderHeaderDto = new();
            CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.cartHeader.Phone = cartDto.cartHeader.Phone;
            cart.cartHeader.Email = cartDto.cartHeader.Email;
            cart.cartHeader.Name = cartDto.cartHeader.Name;

            var response = await _orderRestService.CreateOrder($"{SD.OrderApi}/CreateOrder", cart, true);
            orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Data));

            if (response != null && response.IsSucceeded)
            {
                //get stripe session and redirect to stripe to place order
                //
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";

                StripeRequestDto stripeRequestDto = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CancelUrl = domain + "cart/checkout",
                    OrderHeader = orderHeaderDto
                };

                var stripeResponse = await _orderRestService.CreateStripeSession($"{SD.OrderApi}/CreateStripeSession", stripeRequestDto, true);
                StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>
                                            (Convert.ToString(stripeResponse.Data));
                Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                return new StatusCodeResult(303);

            }
            return View();
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            ResponseDto? response = await _orderRestService.ValidateStripeSession($"{SD.OrderApi}/ValidateStripeSession/{orderId}",true);
            if (response != null & response.IsSucceeded)
            {

                OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Data));
                if (orderHeader.Status == SD.Status_Approved)
                {
                    return View(orderId);
                }
            }
            //redirect to some error page based on status
            return View(orderId);
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
        [HttpPost]
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

        

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cardDto)
        {
            var cartDto = new CartDto();
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
            var result = await _cartService.GetCartByUserIdAsync($"{SD.ShoppingCartApi}/GetCart/{userId}", true);
            if (result.IsSucceeded = false)
            {
                TempData["error"] = result.Message;
                return View(new CartDto());
            }
            cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(result.Data));


            cartDto.cartHeader.Email=User.Claims.Where(x=>x.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
            var response = await _cartService.EmailCart($"{SD.ShoppingCartApi}/EmailCartRequest", cartDto, true);
            if (response.IsSucceeded)
            {
                TempData["success"] = "email will be processed and send shortly";
                return RedirectToAction(nameof(CartIndex));
            }
            return View(nameof(CartIndex));
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.GetCartByUserIdAsync($"{SD.ShoppingCartApi}/GetCart/{userId}", true);
            if (response != null & response.IsSucceeded)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Data));
                return cartDto;
            }
            return new CartDto();
        }

    }
}
