using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mongo.ShoppingCartApi.Model.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mongo.ShoppingCartApi.Model
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartheaderId { get; set; }
        public int productId { get; set; }
        public int Count { get; set; }

        [NotMapped]
        public ProductDto product { get; set; }   

        [ForeignKey(nameof(CartheaderId))]
        public CartHeader? cartHeader { get; set; }

    }
}
