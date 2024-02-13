using static Mongo.Web.Utilites.SD;

namespace Mongo.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.Get;
        public string? Url { get; set; }
        public object? Data { get; set; }
        public string? AccessToken { get; set; }

    }
}
