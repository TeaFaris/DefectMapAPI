using DefectMapAPI.Configurations;
using DefectMapAPI.Models.RefreshTokenModel;
using DefectMapAPI.Models.UserModel;
using DefectMapAPI.Services.JwtTokenGeneratorService.Models;
using DefectMapAPI.Services.Repositories.RefreshToken;
using Microsoft.Extensions.Options;
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
        readonly IRefreshTokenRepository refreshTokenRepository;
        readonly TokenValidationParameters tokenValidationParameters;
        public JwtTokenGenerator(
                IOptions<JwtSettings> jwtSettings,
                IRefreshTokenRepository refreshTokenRepository,
                TokenValidationParameters tokenValidationParameters
            )
        {
            this.tokenValidationParameters = tokenValidationParameters;
            this.refreshTokenRepository = refreshTokenRepository;
            this.jwtSettings = jwtSettings.Value;
        }

        public async Task<JwtTokensResult> GenerateTokens(ApplicationUser applicationUser)
        {
            var securityToken = GenerateSecurityToken(applicationUser);

            var refreshToken = await GenerateRefreshToken(applicationUser, securityToken);
            var jwtToken = SecurityTokenHandler.WriteToken(securityToken);

            return new JwtTokensResult
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken
            };
        }

        private async Task<string> GenerateRefreshToken(ApplicationUser user, JwtSecurityToken token)
        {
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                Token = Guid.NewGuid().ToString(),
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(3),
                Revoked = false,
                Used = false,
                ApplicationUser = user,
            };

            await refreshTokenRepository.AddAsync(refreshToken);
            await refreshTokenRepository.SaveAsync();

            return refreshToken.Token;
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
