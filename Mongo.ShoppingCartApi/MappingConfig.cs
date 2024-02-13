using AutoMapper;
using Mongo.ShoppingCartApi.Model;
using Mongo.ShoppingCartApi.Model.Dto;

namespace Mongo.ShoppingCartApi
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        }
    }
}
