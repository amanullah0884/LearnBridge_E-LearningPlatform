using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // REGISTER PAGE
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER USER
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(registerDto);
            }

            // Add role
            await _userManager.AddToRoleAsync(user, registerDto.Role);

            return RedirectToAction("Login");
        }

        // LOGIN PAGE
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN USER
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var result = await _signInManager.PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                false,
                false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(loginDto);
            }

            return RedirectToAction("Index", "Home");
        }

        // LOGOUT
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}

//using LearnBridge_E_LearningPlatform.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace LearnBridge_E_LearningPlatform.Controllers
//{
//    public class AuthController : Controller
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly JwtService jwtService;

//        public  AuthController(UserManager<ApplicationUser> userManager, JwtService jwtService)
//        {
//            _userManager = userManager;
//            this.jwtService = jwtService;
//        }
//        [HttpPost("register")]
//        public async Task<IActionResult>Register(RegisterDto registerDto)
//        {
//            var user = new ApplicationUser
//            {
//                UserName = registerDto.Email,
//                Email = registerDto.Email,
//                FullName = registerDto.FullName
//            };
//            var result = await _userManager.CreateAsync(user, registerDto.Password);
//            if (!result.Succeeded)
//            {
//                return BadRequest(result.Errors);
//            }
//            await _userManager.AddToRoleAsync(user, registerDto.Role);
//            return Ok(new { Message = "User registered successfully" });
//        }
//        [HttpPost("login")]
//        public async Task<IActionResult> Login(LoginDto loginDto)
//        {
//            var user = await _userManager.FindByEmailAsync(loginDto.Email);
//            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
//            {
//                return Unauthorized(new { Message = "Invalid credentials" });
//            }
//            var roles = await _userManager.GetRolesAsync(user);
//            var token = jwtService.GenerateToken(user, roles);
//            return Ok(new { Token = token });
//        }


//    }
//}
