namespace Mongo.CouponApi.Services.Interfaces
{
    public interface IGenericRepository<T>  where T:class
    {
        Task<List<T>> GetAllAsync();
        Task CreateAsync(T item);
        Task<T> GetByIdAsync(int id);
        void DeleteAsync(T item);
        void Update(T item);
    }
}
