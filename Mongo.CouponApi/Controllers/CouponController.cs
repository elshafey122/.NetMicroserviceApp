using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mongo.CoponApi.Model;
using Mongo.CouponApi.Model;
using Mongo.CouponApi.Services.Interfaces;

namespace Mongo.CouponApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CouponController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
         
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCoupons()
        {
            try
            {
                List<Coupon> coupons = await _unitOfWork.Coupons.GetAllAsync();
                return Ok(new Response(coupons));
            }
            catch(Exception ex)
            {
                return BadRequest(new Response(ex.Message,false));
            }
        }


        [HttpGet("GetByCode/{Code}")]
        public async Task<IActionResult> GetByCode(string Code)
        {
            try
            {
                var result = await _unitOfWork.Coupons.GetByCodeAsyn(Code);
                if (result == null)
                {
                    return NotFound(new Response(""));
                }
                return Ok(new Response(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message,false));
            }
        }

        [HttpPost("Create")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateCoupon([FromBody] CouponDto newcoupon)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(newcoupon);
                var result = _unitOfWork.Coupons.CreateAsync(coupon);
                await _unitOfWork.complete();
                return Ok(new Response(""));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }

        [HttpPut("Edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCoupon(CouponEdit newcoupon)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(newcoupon);
                _unitOfWork.Coupons.Update(coupon);
                await _unitOfWork.complete();
                return Ok(new Response(""));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            try
            {
                var coupon = await _unitOfWork.Coupons.GetByIdAsync(id);
                _unitOfWork.Coupons.DeleteAsync(coupon);
                await _unitOfWork.complete();
                return Ok(new Response(""));
            }
            catch(Exception ex)
            {
                return BadRequest(new Response(ex.Message, false));
            }
        }

    }
}
