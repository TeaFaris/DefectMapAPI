using DefectMapAPI.Models;
using DefectMapAPI.Services.Repositories.User;
using DefectMapAPI.Services.UserAuthenticationManagerService.Models;

namespace DefectMapAPI.Services.UserAuthenticationManagerService
{
    public class UserAuthenticationManager : IUserAuthenticationManager
    {
        readonly IUserRepository userRepository;

        public UserAuthenticationManager(
                IUserRepository userRepository
            )
        {
            this.userRepository = userRepository;
        }

        public async Task<LoginUserResult> ValidateCredentials(string username, string password)
        {
            var usersFound = await userRepository.FindAsync(x => x.Username == username);

            var user = usersFound.FirstOrDefault();

            var isValidPassword = BCrypt.Net.BCrypt.Verify(password, user?.PasswordHash ?? "");

            return new LoginUserResult
            {
                Successful = isValidPassword,
                User = isValidPassword ? user : null
            };
        }

        public async Task<RegisterUserResult> Register(string username, string password)
        {
            var errors = new List<string>();

            var usersWithSameUsername = await userRepository.FindAsync(x => x.Username == username);

            if (usersWithSameUsername.Any())
            {
                errors.Add("This username is already taken.");
            }

            if (errors.Any())
            {
                return new RegisterUserResult
                {
                    Successful = false,
                    Errors = errors
                };
            }

            var newUser = new ApplicationUser
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = UserRoles.User.ToString()
            };

            await userRepository.AddAsync(newUser);

            await userRepository.SaveAsync();

            return new RegisterUserResult
            {
                Successful = true,
                User = newUser
            };
        }
    }
}
