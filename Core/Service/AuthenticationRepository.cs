using Core.DTO;
using Core.Entity;
using Core.Exceptions;
using Core.Helpers;
using Core.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<Admin> userManager;
        private readonly SignInManager<Admin> signInManager;
        private readonly IOptions<JwtOptions> jwtOptions;

        public AuthenticationRepository(UserManager<Admin> userManager, IOptions<JwtOptions> jwtOptions, SignInManager<Admin> signInManager)
        {
            this.userManager = userManager;
            this.jwtOptions = jwtOptions;
            this.signInManager = signInManager;
        }

        public async Task AdminCreateAsync()
        {
            var findUser = await userManager.FindByEmailAsync("solido1a@gmx.de");
            if (findUser != null)
                throw new HttpException("Accaunt already exists.", System.Net.HttpStatusCode.BadRequest);

            var users = new Admin()
            {
                Login = "SolidoAdmin",
                UserName = "SolidoAdmin",
                Email = "solido1a@gmx.de"
            };
            var result = await userManager.CreateAsync(users, "Solidoadmin1234!");
            if (!result.Succeeded)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    stringBuilder.AppendLine(error.Description);
                }
                throw new HttpException(stringBuilder.ToString(), System.Net.HttpStatusCode.InternalServerError);
            }

            var developers = new Admin()
            {
                Login = "Developer",
                UserName = "Developer"
            };
            var resultDev = await userManager.CreateAsync(developers, "Developer123!");
            if (!resultDev.Succeeded)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in resultDev.Errors)
                {
                    stringBuilder.AppendLine(error.Description);
                }
                throw new HttpException(stringBuilder.ToString(), System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<AuthorizationDTO> LoginAsync(string login, string password)
        {
            var users = await userManager.FindByNameAsync(login);
            if (users == null)
                throw new HttpException("Invalid login.", System.Net.HttpStatusCode.BadRequest);
            else if (!await userManager.CheckPasswordAsync(users, password))
                throw new HttpException($"Invalid password.", System.Net.HttpStatusCode.BadRequest);

            return new AuthorizationDTO
            {
                Token = GenerateToken(login)
            };
        }

        public string GenerateToken(string login)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, login)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                            issuer: jwtOptions.Value.Issuer,
                            claims: claims,
                            signingCredentials: credentials
                            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthorizationDTO> ChangeProfile(string login, string currentPassword, string newPassword)
        {
            var profile = await userManager.FindByEmailAsync("solido1a@gmx.de");

            profile.Login = login;
            profile.UserName = login;

            await userManager.UpdateAsync(profile);
            if (currentPassword != null && newPassword != null)
            {
                var users = new Admin()
                {
                    Login = login,
                    UserName = login
                };

                var pass = await userManager.ChangePasswordAsync(users, currentPassword, newPassword);
            }
                

            return new AuthorizationDTO
            {
                Token = GenerateToken(login)
            };
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
