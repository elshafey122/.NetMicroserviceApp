using Microsoft.EntityFrameworkCore;
using Mongo.ProductApi.Data;
using Mongo.ProductApi.Services.Interfaces;
using System.Linq.Expressions;

namespace Mongo.ProductApi.Services.Implemintations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges(); 
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = await _dbSet.AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<T> GetByIdAsync(Expression<Func<T,bool>> expression)
        {
            var product =  await _dbSet.AsNoTracking().FirstOrDefaultAsync(expression);
            return product;
        }
    }
}
