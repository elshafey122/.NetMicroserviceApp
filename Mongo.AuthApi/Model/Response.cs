using System.Net;

namespace Mongo.AuthApi.Model
{
    public class Response
    {
        public object? Data { get; set; } ="";
        public bool IsSucceeded { get; set; } = true;
        public string? Message { get; set; } = "";
        //public Response(string _message,bool _isSucceed)
        //{
        //    Message = _message;
        //    IsSucceeded = false;
        //}
        //public Response(object _data) 
        //{
        //    Data = _data;
        //}


    }
}
