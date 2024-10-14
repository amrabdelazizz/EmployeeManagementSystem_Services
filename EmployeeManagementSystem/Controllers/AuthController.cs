using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtService _jwtService;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, JwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        [Authorize (Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.Password != model.ConfimPassword)
            {
                return BadRequest("Password Doesnot match");
            }

            if (!new[] { "admin", "user" }.Contains(model.Role.ToLower()))
            {
                return BadRequest("Role is incorrect. Allowed values: [User, Admin]");
            }
            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // get user roles
                var roles = await _userManager.GetRolesAsync(user);
                // get token
                var token =  await _jwtService.CreateJwtTokenAsync(user);
                return Ok(new
                {
                    token = token,
                    roles = roles
                   
                });
            }

            return Unauthorized("User Name Or Password is incorrect ");
        }
    }
}
