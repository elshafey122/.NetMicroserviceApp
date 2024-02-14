using Microsoft.AspNetCore.Mvc;
using Mongo.Web.Model.Product;
using Mongo.Web.Services.Interfaces;
using Mongo.Web.Utilites;
using Newtonsoft.Json;

namespace Mongo.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IPrductRestService _productRestService;
        public ProductController(IPrductRestService prductRestService)
        {
			_productRestService = prductRestService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> products = new();
            var result = await _productRestService.GetAsync($"{SD.ProductApi}/GetAll", true);
            if (!result.IsSucceeded)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index","Home");
            }
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(result.Data));
            return View(products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            ProductDto productdto = new();
			return View("ProductForm",productdto);
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto newproduct)
        {
			if (ModelState.IsValid)
            {
                var response = await _productRestService.CreateAsync($"{SD.ProductApi}/Create", newproduct, true);
                if (response.IsSucceeded)
                {
                    TempData["success"] = "created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
				TempData["error"] = response.Message;
				return View("ProductForm", newproduct);
			}
            TempData["error"] = "try again";
            return View("ProductForm", newproduct);
        }

        public async Task<IActionResult> ProductUpdate(int productId)
        {
            var result = await _productRestService.GetAsync($"{SD.ProductApi}/GetById/{productId}", true);
            if (!result.IsSucceeded)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(result.Data));
            product.ProductId = productId;
            return View("ProductForm", product);
        }


        [HttpPost]
		public async Task<IActionResult> ProductUpdate(ProductDto newproduct)
		{
			if (ModelState.IsValid)
			{
				var response = await _productRestService.UpdateAsync($"{SD.ProductApi}/Update", newproduct, true);
				if (response.IsSucceeded)
				{
					TempData["success"] = "updated successfully";
					return RedirectToAction(nameof(ProductIndex));
				}
				TempData["error"] = response.Message;
				return View("ProductForm", newproduct);
			}
			TempData["error"] = "try again";
			return View("ProductForm", newproduct);
		}


		public async Task<IActionResult> ProductDelete(int productId)
        {
			var response = await _productRestService.DeleteAsync($"{SD.ProductApi}/Delete/{productId}", true);
            if (!response.IsSucceeded)
            {
                TempData["error"] = response.Message;
                return RedirectToAction(nameof(ProductIndex));
            }
            TempData["success"] = "deleted successfully";
			return RedirectToAction(nameof(ProductIndex));
		}

	}
}
