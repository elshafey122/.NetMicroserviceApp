﻿using System.ComponentModel.DataAnnotations;

namespace Mongo.ProductApi.Model
{
    public class ProductDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}