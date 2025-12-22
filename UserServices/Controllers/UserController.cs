using Microsoft.AspNetCore.Mvc;
using UserServices.Service;
using static UserServices.DTO.UserDTO;

namespace UserServices.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Signup(request);
            if (result == "Email already exists. Please use a different email")
            {
                return Conflict(result);
            }
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var result = await _userService.Login(request);

            if (result == null)
                return Unauthorized(new { message = "Login failed" });

            return Ok(new { token = result.Token, message = "Login successful" });
        }
    }
}
