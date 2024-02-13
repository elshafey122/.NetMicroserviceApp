using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mongo.AuthApi.Model;
using Mongo.AuthApi.Services.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mongo.AuthApi.Services.Repositories
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JWTOptions _jwtoptions;
        public JwtTokenGenerator(IOptions<JWTOptions> jwtOptions)
        {
            _jwtoptions = jwtOptions.Value;
        }

        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            var Tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtoptions.SecretKey);

            var claimList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
            };
            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokendescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtoptions.Audience,
                Issuer = _jwtoptions.Iusser,
                Expires = DateTime.Now.AddMinutes(30),
                Subject = new ClaimsIdentity(claimList),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = Tokenhandler.CreateToken(tokendescriptor);
            return Tokenhandler.WriteToken(token);
        }
    }
}
