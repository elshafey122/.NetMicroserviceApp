using Microsoft.EntityFrameworkCore;
using Mongo.CoponApi.Data;
using Mongo.CouponApi.Services.Interfaces;

namespace Mongo.CouponApi.Services.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteAsync(T item)
        { 
             _context.Set<T>().Remove(item);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = await _context.Set<T>().AsNoTracking().ToListAsync();
            return result;
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }

        public async Task CreateAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
