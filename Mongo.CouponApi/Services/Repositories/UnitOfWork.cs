using Mongo.CoponApi.Data;
using Mongo.CouponApi.Services.Interfaces;

namespace Mongo.CouponApi.Services.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICouponRepository Coupons { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Coupons = new CouponRepository(context);

        }

        public async Task<int> complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
