using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mongo.Model.Cart.Dto;
using Mongo.Web.Model.Product;
using Mongo.Web.Models;
using Mongo.Web.Services.Interfaces;
using Mongo.Web.Utilites;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mongo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPrductRestService _productRestService;
        private readonly ICartRestService _cartService;
        public HomeController(IPrductRestService prductRestService, ICartRestService cartService)
        {
            _productRestService = prductRestService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = new();
            var result = await _productRestService.GetAsync($"{SD.ProductApi}/GetAll", true);
            if (!result.IsSucceeded)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(result.Data));
            return View(products);
        }

        public async Task<IActionResult> HomeDetails(int ProductId)
        {
            var result = await _productRestService.GetAsync($"{SD.ProductApi}/GetById/{ProductId}", true);
            if (!result.IsSucceeded)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(result.Data));
            return View(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> HomeDetails(ProductDto ProductDto)
        {
            CartDto cartDto = new CartDto()
            {
                cartHeader = new CartHeaderDto()
                {
                    UserId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value,
                }
            };
            CartDetailsDto cartDetailsDto = new CartDetailsDto()
            {
                productId = ProductDto.ProductId,
                Count = ProductDto.Count,
            };
            List<CartDetailsDto> cartDetailsDtos = new List<CartDetailsDto> { cartDetailsDto };
            cartDto.cartDetails = cartDetailsDtos;
            var result = await _cartService.UpsertCartAsync($"{SD.ShoppingCartApi}/CartUpsert" ,cartDto , true);
            if (!result.IsSucceeded)
            {
                TempData["error"] = result.Message;
                return View(ProductDto);
            }
            TempData["success"] = "item has added to cart successfully";
            return RedirectToAction(nameof(Index));

        }

    }
}
