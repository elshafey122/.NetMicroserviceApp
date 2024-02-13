namespace Mongo.AuthApi.Model
{
    public class JWTOptions
    {
        public string SecretKey { get; set; }=string.Empty;
        public string Iusser { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}
