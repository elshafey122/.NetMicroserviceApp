using Newtonsoft.Json;

namespace Mongo.Web.Model.Order.Dto
{
    public class OrderHeaderDto
    {
        [JsonProperty("orderHeaderId")]
        public int OrderHeaderId { get; set; }

        [JsonProperty("userId")]
        public string? UserId { get; set; }

        [JsonProperty("couponCode")]
        public string? CouponCode { get; set; }

        [JsonProperty("discount")]
        public double Discount { get; set; }

        [JsonProperty("orderTotal")]
        public double OrderTotal { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("orderTime")]
        public DateTime OrderTime { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("paymentIntentId")]
        public string? PaymentIntentId { get; set; }

        [JsonProperty("stripeSessionId")]
        public string? StripeSessionId { get; set; }

        [JsonProperty("orderDetails")]
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
    }
}
