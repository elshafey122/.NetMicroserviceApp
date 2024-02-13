using System.Net;

namespace Mongo.ShoppingCartApi.Model
{
    public class ResponseDto
    {
        public object Data { get; set; }
        public bool IsSucceeded { get; set; } = true;
        public string? Message { get; set; } = "";


    }
}
