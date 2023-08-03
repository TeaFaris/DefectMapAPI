namespace DefectMapAPI.Models.ApiResponses
{
    public class LoginResponse
    {
        public bool Successful { get; init; }
        public string? JwtToken { get; init; }
        public string? RefreshToken { get; init; }
    }
}
