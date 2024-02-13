using Microsoft.AspNetCore.Identity;

namespace Mongo.AuthApi.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
