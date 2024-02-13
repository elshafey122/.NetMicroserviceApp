using Mongo.Web.Model.Auth;

namespace Mongo.Web.Model.Auth
{
    public class LoginResponse:UserDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
