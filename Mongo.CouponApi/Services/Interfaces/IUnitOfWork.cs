namespace Mongo.CouponApi.Services.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        public ICouponRepository Coupons { get; }
        Task<int> complete();
    }
}
