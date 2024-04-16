using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Mongo.AuthApi.Model;
using Mongo.AuthApi.Model.Dto;
using Mongo.AuthApi.Services.Interfaces;

namespace Mongo.AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly Response _response;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
            _response=new Response();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequest registerRequest)
        {
            var result = await _authRepository.RegisterAsync(registerRequest);
            if (result.IsSucceeded == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequests loginRequest)
        {
            var result = await _authRepository.LoginAsync(loginRequest);
            if (result.User==null)
            {
                _response.IsSucceeded = false;
                _response.Message = "Usename Or Password is invalid";
                return BadRequest(_response);
            }
            _response.Data = result;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(string email,string role)
        {
            var result = await _authRepository.AssignRole(email,role);
            if (result.IsSucceeded == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
