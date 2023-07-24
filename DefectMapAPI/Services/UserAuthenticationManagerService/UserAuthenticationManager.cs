using DefectMapAPI.Models;
using DefectMapAPI.Models.Shared.LoginUser;
using DefectMapAPI.Models.Shared.RegisterUser;
using DefectMapAPI.Models.Shared.Requests;
using DefectMapAPI.Services.Repositories.User;
using System.ComponentModel.DataAnnotations;

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

        public async Task<LoginUserResponse> ValidateCredentials(LoginUserRequest loginRequest)
        {
            var usersFound = await userRepository.FindAsync(x => x.Username == loginRequest.Username);

            var user = usersFound.FirstOrDefault();

            var isValidPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user?.PasswordHash);

            return new LoginUserResponse
            {
                Successful = isValidPassword
            };
        }

        public async Task<RegisterUserResponse> Register(RegisterUserRequest registerRequest)
        {
            var errors = new List<string>();

            var validationContext = new ValidationContext(registerRequest);
            ICollection<ValidationResult>? validationResults = null;

            if (!Validator.TryValidateObject(registerRequest, validationContext, validationResults, true))
            {
                errors.AddRange(validationResults!
                                    .Where(x => x.ErrorMessage is not null)
                                    .Select(x => x.ErrorMessage!));
            }

            var usersWithSameUsername = await userRepository.FindAsync(x => x.Username == registerRequest.Username);

            if (usersWithSameUsername.Any())
            {
                errors.Add("This username is already taken.");
            }

            if (errors.Any())
            {
                return new RegisterUserResponse
                {
                    Successful = false,
                    Errors = errors
                };
            }

            var newUser = new ApplicationUser
            {
                Username = registerRequest.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password)
            };

            return new RegisterUserResponse
            {
                Successful = true,
                User = newUser
            };
        }
    }
}
