namespace Mongo.AuthApi.Model.Dto
{
    public class LoginResponse
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
