using Microsoft.EntityFrameworkCore;
using Mongo.CoponApi.Data;
using Mongo.CoponApi.Model;
using Mongo.CouponApi.Services.Interfaces;

namespace Mongo.CouponApi.Services.Repositories
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }


        public async Task<Coupon> GetByCodeAsyn(string code)
        {
            var result = await _context.Coupons.FirstOrDefaultAsync(x => x.CouponCode.ToLower() == code.ToLower());
            return result;
        }
    }
}