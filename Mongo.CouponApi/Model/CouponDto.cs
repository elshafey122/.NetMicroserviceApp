using System.ComponentModel.DataAnnotations;

namespace Mongo.CouponApi.Model
{
    public class CouponDto
    {
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
