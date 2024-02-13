using Mongo.Web.Model;

namespace Mongo.Web.Services.Interfaces
{
    public interface IRestService<T> where T : class
    {
        Task<ResponseDto> GetAsync(string url, bool withbearer = true);
        Task<ResponseDto> GetByIdAsync(string url, bool withbearer = true);
        Task<ResponseDto> CreateAsync(string url, T data, bool withbearer = true);
        Task<ResponseDto> UpdateAsync(string url, T data, bool withbearer = true);
        Task<ResponseDto> DeleteAsync(string url, bool withbearer = true);
    }
}
