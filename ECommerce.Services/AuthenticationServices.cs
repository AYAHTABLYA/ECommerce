using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.ServicesAbstarction;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.IdentityDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationServices(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (User == null)
                return Error.InvalidCredentials("Invalid Email or Password!");

            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);
            if (!IsPasswordValid)
                return Error.InvalidCredentials("Invalid Password!");
            var Token = await CreateTokenAsync(User);

            return new UserDTO(User.Email!, User.DisplayName, Token);
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber
            };


            var IdentityResult = await _userManager.CreateAsync(User, registerDTO.Password);

            if (IdentityResult.Succeeded)
            {
                var Token = await CreateTokenAsync(User);
                return new UserDTO(User.Email!, User.DisplayName, Token);
            }

            return IdentityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            // Claims
            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!)
            };

            // Roles
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim("roles", role));
            }

            // Secret Key
            var SecretKey = _configuration["JWTOptions:SecretKey"];

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            //Signing Credentials
            var Cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            // Create Token
            var Token = new JwtSecurityToken(
                            issuer: _configuration["JWTOptions:Issuer"],
                            audience: _configuration["JWTOptions:Audience"],
                            claims: Claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: Cred
            );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            return User != null;
        }

        public async Task<Result<UserDTO>> GetUserEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);

            if (User == null)
                return Error.NotFound("User not found!");
            return new UserDTO(User.Email!, User.DisplayName, await CreateTokenAsync(User));
        }
    }
}
