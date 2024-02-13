using System.ComponentModel.DataAnnotations;

namespace Mongo.ShoppingCartApi.Model
{
    public class CouponDto
    {
        //public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
