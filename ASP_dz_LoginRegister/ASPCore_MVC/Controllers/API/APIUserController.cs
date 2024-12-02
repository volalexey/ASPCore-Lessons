using ASPCore_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASPCore_MVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIUserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public APIUserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]AuthModel model)
        {
            Console.WriteLine("We're in the Create function!!");
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Both Email and password are required");
            }
            var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok("Registered successfully");
            }
            //logs errors
            return BadRequest(result.Errors);
        }
        [HttpPost("auth")]
        public async Task<IActionResult> Auth([FromBody]AuthModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Both Email and password are required");
            }

            var res = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (res.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = _generateJwtToken(user);
                return Ok(new { token = token });
            }
            else
            {
                return BadRequest("Authorisation failed. Check credentials.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody]string roleName)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.FindByNameAsync(User.Identity.Name), "Admin"))
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    return BadRequest("Empty role name.");
                }
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var res = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (res.Succeeded)
                    {
                        return Ok($"Role {roleName} was created");
                    }
                    else
                    {
                        return BadRequest(Json(res));
                    }

                }
                else
                {
                    return BadRequest("Role with such name already exists.");
                }
            }
            return BadRequest("Admin only");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("roles/assign")]
        public async Task<IActionResult> AssignRole(string name, string roleName)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.FindByNameAsync(User.Identity.Name), "Admin"))
            {
                if (string.IsNullOrWhiteSpace(roleName) || string.IsNullOrWhiteSpace(roleName))
                {
                    return BadRequest("Empty user or role name.");
                }
                var user = await _userManager.FindByNameAsync(name);
                if (await _roleManager.RoleExistsAsync(roleName) && user != null)
                {
                    var res = await _userManager.AddToRoleAsync(user, roleName);

                    if (res.Succeeded)
                    {
                        return Ok($"{name} is now {roleName}");
                    }
                    else
                    {
                        return BadRequest(Json(res));
                    }

                }
                else
                {
                    return BadRequest("Role or user with such name doesn't exists.");
                }
            }
            return BadRequest("Admin only");
        }
        public async Task<IActionResult> AccessDenied()
        {
            return BadRequest("Cookie: Access Denied");
        }
        private string _generateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
