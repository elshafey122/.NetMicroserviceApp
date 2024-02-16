using Mongo.EmailApi.Model.Dto;

namespace Mongo.EmailApi.NewFolder
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
    }
}
