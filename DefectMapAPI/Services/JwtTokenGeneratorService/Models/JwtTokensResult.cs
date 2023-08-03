namespace DefectMapAPI.Services.JwtTokenGeneratorService.Models
{
    public class JwtTokensResult
    {
        public string JwtToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
