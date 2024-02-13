using Microsoft.AspNetCore.Mvc;
using Mongo.Web.Model;
using Mongo.Web.Models.Coupon;
using Mongo.Web.Services.Interfaces;
using Mongo.Web.Utilites;
using Newtonsoft.Json;

namespace Mongo.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponRestService _couponRestService;
        public CouponController(ICouponRestService couponRestService)
        {
            _couponRestService = couponRestService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<Coupon>? coupons = new();

            ResponseDto response = await _couponRestService.GetAsync($"{SD.CouponApi}/GetAll");
            if (response!=null && response.IsSucceeded == true)
            {
                coupons = JsonConvert.DeserializeObject<List<Coupon>>(Convert.ToString(response.Data));
                return View(coupons);
            }
            else
            {
                TempData["error"] = response.Message;
                return RedirectToAction("Index", "Home");
            }
        }


		public async Task<IActionResult> CouponCreate()
		{
            return View();
		}

        [HttpPost]
        public async Task<IActionResult> CouponCreate(Coupon model)
        {
            if(ModelState.IsValid)
            {
                ResponseDto response = await _couponRestService.CreateAsync($"{SD.CouponApi}/Create",model);
                if (response != null && response.IsSucceeded == true)
                {
                    TempData["success"] = "created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response.Message;
					return View(model);
				}
			}
            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
			ResponseDto response = await _couponRestService.DeleteAsync($"{SD.CouponApi}/Delete/{couponId}");
            if (response != null && response.IsSucceeded == true)
            {
                TempData["success"] = "created successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            TempData["error"] = response.Message;
            return RedirectToAction(nameof(CouponIndex));

        }
    }
}
