using DefectMapAPI.Configurations;
using DefectMapAPI.Models.UserModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DefectMapAPI.Services.JwtTokenGeneratorService
{
    public class JwtTokenGenerator
    {
        private static readonly JwtSecurityTokenHandler SecurityTokenHandler = new();

        readonly JwtSettings jwtSettings;
        public JwtTokenGenerator(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }

        public string GenerateToken(ApplicationUser applicationUser)
        {
            var securityToken = GenerateSecurityToken(applicationUser);

            return SecurityTokenHandler.WriteToken(securityToken);
        }

        private JwtSecurityToken GenerateSecurityToken(ApplicationUser applicationUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, applicationUser.Id.ToString()),
                new Claim(ClaimTypes.Name, applicationUser.Username),
                new Claim(ClaimTypes.Role, applicationUser.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: DateTime.Now.Add(jwtSettings.ExpiryTime),
                signingCredentials: credentials);

            return token;
        }
    }
}
