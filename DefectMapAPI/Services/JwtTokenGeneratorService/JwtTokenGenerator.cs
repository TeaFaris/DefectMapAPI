using DefectMapAPI.Configurations;
using DefectMapAPI.Models.RefreshTokenModel;
using DefectMapAPI.Models.UserModel;
using DefectMapAPI.Services.JwtTokenGeneratorService.Models;
using DefectMapAPI.Services.Repositories.RefreshToken;
using DefectMapAPI.Services.Repositories.User;
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
        readonly IUserRepository userRepository;
        public JwtTokenGenerator(
                IOptions<JwtSettings> jwtSettings,
                IRefreshTokenRepository refreshTokenRepository,
                IUserRepository userRepository,
                TokenValidationParameters tokenValidationParameters
            )
        {
            this.userRepository = userRepository;
            this.tokenValidationParameters = tokenValidationParameters;
            this.refreshTokenRepository = refreshTokenRepository;
            this.jwtSettings = jwtSettings.Value;
        }

        public async Task<JwtTokens?> VerifyAndGenerateTokens(string refreshToken)
        {
            var tokensFound = await refreshTokenRepository.FindAsync(x => x.Token == refreshToken);
            var storedRefreshToken = tokensFound.FirstOrDefault();

            if (storedRefreshToken is null
                or { Revoked: true }
                or { Used: true }
                || DateTime.UtcNow > storedRefreshToken.ExpiryDate
               )
            {
                return null;
            }

            storedRefreshToken.Used = true;
            await refreshTokenRepository.UpdateAsync(storedRefreshToken);
            await refreshTokenRepository.SaveAsync();

            var user = (await userRepository.GetAsync(storedRefreshToken.UserId))!;

            return await GenerateTokens(user);
        }

        public async Task<JwtTokens> GenerateTokens(ApplicationUser applicationUser)
        {
            var securityToken = GenerateSecurityToken(applicationUser);

            var refreshToken = await GenerateRefreshToken(applicationUser, securityToken);
            var jwtToken = SecurityTokenHandler.WriteToken(securityToken);

            return new JwtTokens
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
