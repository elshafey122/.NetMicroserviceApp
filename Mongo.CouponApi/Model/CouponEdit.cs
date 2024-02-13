using System.ComponentModel.DataAnnotations;

namespace Mongo.CouponApi.Model
{
    public class CouponEdit
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
