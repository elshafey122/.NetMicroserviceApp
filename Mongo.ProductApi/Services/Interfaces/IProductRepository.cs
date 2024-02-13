using Mongo.ProductApi.Model;

namespace Mongo.ProductApi.Services.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task Update(Product product);
    }
}
