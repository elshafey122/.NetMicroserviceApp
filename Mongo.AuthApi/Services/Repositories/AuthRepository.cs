using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mongo.AuthApi.Data;
using Mongo.AuthApi.Model;
using Mongo.AuthApi.Model.Dto;
using Mongo.AuthApi.Services.Interfaces;

namespace Mongo.CouponApi.Services.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly Response _response;
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtokengenerator;

        public AuthRepository(ApplicationDbContext context, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtokengenerator)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtokengenerator=jwtokengenerator;
            _response = new Response();
        }
        public async Task<Response> RegisterAsync(RegisterationRequest registerRequest)
        {
            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = registerRequest.Email,
                NormalizedEmail=registerRequest.Email.ToUpper(),
                PhoneNumber = registerRequest.PhoneNumber,
                Name = registerRequest.Name,
                UserName = registerRequest.UserName,
            };
            try
            {
                var result = await _userManager.CreateAsync(applicationUser, registerRequest.Password);
                if (result.Succeeded)
                {
                    var ReturnUser = await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName == registerRequest.UserName);
                    UserDto UserDto = new UserDto()
                    {
                        Email = ReturnUser.Email,
                        Name = ReturnUser.Name,
                        PhoneNumber = ReturnUser.PhoneNumber,
                        UserName = ReturnUser.UserName,
                        Id=ReturnUser.Id
                    };
                    _response.Data = UserDto;
                    await AssignRole(registerRequest.Email, "Customer");
                }
                else
                {
                    _response.IsSucceeded = false;
                    _response.Message = result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                _response.IsSucceeded = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        public async Task<LoginResponse> LoginAsync(LoginRequests loginRequest)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequest.UserName.ToLower());
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if(user==null || checkPassword == false)
            {
                return new LoginResponse()
                {
                    User = null,
                    Token = "",
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtokengenerator.GenerateToken(user,roles);

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            };

            return new LoginResponse()
            {
                Token = token,
                User = userDto
            };
        }

        public async Task<Response> AssignRole(string email, string role)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                _response.IsSucceeded = false;
                _response.Message = "User not found";
            }
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            await _userManager.AddToRoleAsync(user, role);
            return _response;
        }
    }
}