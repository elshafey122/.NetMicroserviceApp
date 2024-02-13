using AutoMapper;
using Mongo.ProductApi.Model;

namespace Mongo.ProductApi
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductEdit, Product>().ReverseMap();
    }
}
}
