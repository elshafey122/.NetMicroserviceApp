using Mongo.CoponApi.Model;

namespace Mongo.CouponApi.Services.Interfaces
{
    public interface ICouponRepository:IGenericRepository<Coupon>
    {
        Task<Coupon> GetByCodeAsyn(string code);
    }
}
