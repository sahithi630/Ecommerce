using static UserServices.DTO.UserDTO;

namespace UserServices.Service
{
    public interface IUserService
    {
        Task<LoginResponseDto?> Login(LoginDto request);

        Task<string> Signup(SignupDto request);
    }
}
