using Mongo.Web.Models.Coupon;
using Mongo.Web.Services.Interfaces;

namespace Mongo.Web.Services.Implemintations
{
    public class CouponRestService : RestService<Coupon>, ICouponRestService
    {
        public CouponRestService(ITokenProvider tokenProvider) : base(tokenProvider)
        {

        }
    }
}
