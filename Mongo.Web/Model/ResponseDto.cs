using System.Net;

namespace Mongo.Web.Model
{
    public class ResponseDto 
    {
        public object Data { get; set; } 
        public bool IsSucceeded { get; set; } = true;
        public string? Message { get; set; } = "";
        //public Response(string _message,bool _isSucceed)
        //{
        //    Message = _message;
        //    IsSucceeded = false;
        //}
        //public Response(T _data) 
        //{
        //    Data = _data;
        //}


    }
}
