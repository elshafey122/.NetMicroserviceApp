using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mongo.ShoppingCartApi.Model.Dto
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartheaderId { get; set; }
        public int productId { get; set; }
        public int Count { get; set; }
        public ProductDto? product { get; set; }
        public CartHeader? cartHeader { get; set; }
    }
}
