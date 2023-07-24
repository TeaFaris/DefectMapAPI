namespace DefectMapAPI.Models.Shared.RegisterUser
{
    public class RegisterUserResponse
    {
        public bool Successful { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
