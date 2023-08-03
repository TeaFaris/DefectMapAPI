namespace DefectMapAPI.Configurations
{
    public class JwtSettings
    {
        public string Key { get; init; }
        public string Audience { get; init; }
        public string Issuer { get; init; }
        public TimeSpan ExpiryTime { get; init; }
    }
}
