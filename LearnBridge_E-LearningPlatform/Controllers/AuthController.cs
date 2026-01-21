using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService jwtService;

        public  AuthController(UserManager<ApplicationUser> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            this.jwtService = jwtService;
        }
        [HttpPost("register")]
        public async Task<IActionResult>Register(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(user, registerDto.Role);
            return Ok(new { Message = "User registered successfully" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = jwtService.GenerateToken(user, roles);
            return Ok(new { Token = token });
        }

    
    }
}
