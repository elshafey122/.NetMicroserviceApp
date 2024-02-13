using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mongo.Model.Cart.Dto
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public int? UserId { get; set; }
        public string? CouponCode { get; set; }

        public double Discount { get; set; }
        public double CartTotal { get; set; }
    }
}
