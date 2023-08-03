namespace DefectMapAPI.Models.UserModel
{
    public static class UserMapper
    {
        public static UserDTO ToDTO(this ApplicationUser applicationUser)
        {
            return new UserDTO
            {
                Id = applicationUser.Id,
                Username = applicationUser.Username,
                Role = applicationUser.Role
            };
        }
    }
}
