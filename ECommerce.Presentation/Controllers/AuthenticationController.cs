using ECommerce.ServicesAbstarction;
using ECommerce.Shared.DTOS.IdentityDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationServices _authenticationServices;

        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var Result = await _authenticationServices.LoginAsync(loginDTO);
            return HandleResult(Result);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO )
        {
            var Result = await _authenticationServices.RegisterAsync(registerDTO);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var Result = await _authenticationServices.CheckEmailAsync(email);
            return Ok(Result);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Result = await _authenticationServices.GetUserEmailAsync(Email!);
            return HandleResult(Result);
        }
    }
}
