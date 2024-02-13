using AutoMapper;
using Mongo.CoponApi.Model;
using Mongo.CouponApi.Model;

namespace Mongo.CouponApi
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<CouponDto, Coupon>();
            CreateMap<CouponEdit, Coupon>();
        }
    }
}
