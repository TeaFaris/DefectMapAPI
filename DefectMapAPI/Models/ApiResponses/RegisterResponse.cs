namespace DefectMapAPI.Models.ApiResponses
{
    public class RegisterResponse
    {
        public bool Successful { get; init; }
        public IEnumerable<string>? Errors { get; init; }
    }
}
