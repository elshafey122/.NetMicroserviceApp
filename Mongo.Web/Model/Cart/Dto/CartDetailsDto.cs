﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mongo.Web.Model.Product;

namespace Mongo.Model.Cart.Dto
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartheaderId { get; set; }
        public int productId { get; set; }
        public int Count { get; set; }
        public ProductDto? product { get; set; }
        public CartHeaderDto? cartHeader { get; set; }
    }
}
