using DefectMapAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DefectMapAPI.Services.JwtTokenGeneratorService
{
    public class JwtTokenGenerator
    {
        private static readonly JwtSecurityTokenHandler SecurityTokenHandler = new();

        readonly IConfiguration config;
        public JwtTokenGenerator(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateToken(ApplicationUser applicationUser)
        {
            var securityToken = GenerateSecurityToken(applicationUser);

            return SecurityTokenHandler.WriteToken(securityToken);
        }

        private JwtSecurityToken GenerateSecurityToken(ApplicationUser applicationUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, applicationUser.Id.ToString()),
                new Claim(ClaimTypes.Name, applicationUser.Username)
            };

            var token = new JwtSecurityToken(config["JwtSettings:Issuer"],
                config["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return token;
        }
    }
}
