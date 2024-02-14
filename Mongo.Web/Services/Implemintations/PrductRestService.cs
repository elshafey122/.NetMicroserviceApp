using Mongo.Web.Model.Product;
using Mongo.Web.Services.Interfaces;

namespace Mongo.Web.Services.Implemintations
{
    public class PrductRestService : RestService<ProductDto>, IPrductRestService
    {
        public PrductRestService(ITokenProvider tokenProvider) : base(tokenProvider)
        {

        }

    }
}
