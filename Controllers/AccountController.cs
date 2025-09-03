using APITest.DTO;
using APITest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace APITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUSER> _userManager;
        private readonly IConfiguration configuration;
        public AccountController(UserManager<AppUSER> user, IConfiguration configuration)
        {
            _userManager = user;
            this.configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(DtoNewUser newuser)
        {
            if (ModelState.IsValid)
            {
                AppUSER appUSER = new()
                {
                    UserName = newuser.UserName,
                    Email = newuser.Email,
                };
                IdentityResult result = await _userManager.CreateAsync(appUSER, newuser.Password);
                if (result.Succeeded)
                {
                    return Ok("Success");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(DtoLogin login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(login.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("" , "User Name is invalid");
                }
                else
                {
                    if (await _userManager.CheckPasswordAsync(user, login.Password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        { 
                        claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Security"]));
                        var sc = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
                        var token = new JwtSecurityToken(
                           claims: claims,
                           issuer: configuration["JWT:Issure"],
                           audience: configuration["JWT:Audience"],
                           expires: DateTime.Now.AddHours(1),
                           signingCredentials: sc
                            );
                        var _token = new
                        { 
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        };
                        return Ok(_token);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }

            }
            return BadRequest(ModelState);
        }

        
    }
}
