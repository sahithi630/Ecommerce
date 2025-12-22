using System.Security.Cryptography;
using System.Text;
using UserServices.Models;
using UserServices.Repositories;
using static UserServices.DTO.UserDTO;

namespace UserServices.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, IConfiguration configuration)

        {

            _userRepository = userRepository;

            _jwtService = new JwtService(configuration);

        }

        public async Task<string> Signup(SignupDto request)

        {

            if (await _userRepository.GetUserByEmail(request.UserEmail) != null)

                return "Email already exists";

            var hashedPassword = HashPassword(request.Password);

            var user = new User

            {

                UserName = request.UserName,

                UserEmail = request.UserEmail,

                UserContactNumber = request.UserContactNumber,

                PasswordHash = hashedPassword,

                //Role = request.Role

            };

            await _userRepository.AddUser(user);

            return "User registered successfully";

        }

        public async Task<LoginResponseDto?> Login(LoginDto request)

        {

            var user = await _userRepository.GetUserByEmail(request.Email);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))

                return null;

            var token = _jwtService.GenerateJwtToken(user);

            return new LoginResponseDto

            {

                Token = token,

                //Role = user.Role

            };

        }


        private string HashPassword(string password)

        {

            using (var sha256 = SHA256.Create())

            {

                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                return Convert.ToBase64String(bytes);

            }

        }

        private bool VerifyPassword(string password, string storedHash)

        {

            return HashPassword(password) == storedHash;

        }
    }
}
