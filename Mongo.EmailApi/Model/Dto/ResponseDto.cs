using System.Net;

namespace Mongo.EmailApi.Model.Dto
{
    public class ResponseDto
    {
        public object Data { get; set; }
        public bool IsSucceeded { get; set; } = true;
        public string? Message { get; set; } = "";
    }
}