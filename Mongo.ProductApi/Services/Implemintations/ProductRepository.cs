using Microsoft.EntityFrameworkCore;
using Mongo.ProductApi.Data;
using Mongo.ProductApi.Model;
using Mongo.ProductApi.Services.Interfaces;

namespace Mongo.ProductApi.Services.Implemintations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context=context;
        }
        public async Task Update(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
