using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mongo.Web.Model.Auth;
using Mongo.Web.Services.Interfaces;
using Mongo.Web.Utilites;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mongo.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRestService _authRestService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthRestService authRestService, ITokenProvider tokenProvider)
        {
            _authRestService = authRestService;
            _tokenProvider = tokenProvider;
        }
        public IActionResult Login()
        {
            LoginRequests loginRequst = new();
            return View(loginRequst);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequests loginRequest)
        {
            if (!ModelState.IsValid) // model refers to loginrequest object
            {
                return RedirectToAction(nameof(Login));
            }

            var response = await _authRestService.LoginAsync($"{SD.AuthApi}/Login", loginRequest);
            if (!response.IsSucceeded)
            {
                TempData["error"] = response.Message;
                return View(loginRequest);
            }
            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Convert.ToString(response.Data));
            await SignInUser(loginResponse);
            _tokenProvider.SetToken(loginResponse.Token); // used to handle requests and authorization

            TempData["success"] = "login successfully";
            return RedirectToAction("Index","Home");
        }


        public IActionResult Register()
        {
            //var rolelist = new List<SelectListItem>()
            //{
            //    new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
            //    new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
            //};
            //ViewBag.Rolelist=rolelist;

            var c = 33;
            RegisterationRequest registerationRequest = new();
            return View(registerationRequest);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequest registerationRequest)
        {
            if(!ModelState.IsValid)
            {
                return View(registerationRequest);
            }

            var response = await _authRestService.RegisterAsync($"{SD.AuthApi}/Register", registerationRequest);
            if (!response.IsSucceeded && response != null)
            {
                TempData["error"] = response.Message;
                return View(registerationRequest);
            }

            TempData["success"] = "registration successfully";
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home"); 
        }

        private async Task SignInUser(LoginResponse model)// signin user using identity
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

    }
}