using API.Context;
using API.Models;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MyContext _myContext;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(MyContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            _myContext = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        SmtpClient client = new SmtpClient();
        RandomGenerator randomGenerator = new RandomGenerator();
        ServiceEmail serviceEmail = new ServiceEmail();

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterVM registerVM)
        {
            var theCode = randomGenerator.GenerateRandom().ToString();
            serviceEmail.SendEmail(registerVM.Email, theCode);
            var pwHashed = BCrypt.Net.BCrypt.HashPassword(registerVM.Password, 12);
            var user = new User
            {
                Email = registerVM.Email,
                PasswordHash = pwHashed,
                UserName = registerVM.Username,
                EmailConfirmed = false,
                PhoneNumber = registerVM.Phone,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                SecurityStamp = theCode,
                AccessFailedCount = 0
            };
            _myContext.Users.AddAsync(user);
            var role = new UserRole
            {
                UserId = user.Id,
                RoleId = "5"
            };
            _myContext.UserRoles.AddAsync(role);
            _myContext.SaveChanges();
            return Ok("Registered successfully");

        }

        [HttpPost]
        [Route("verify")]
        public async Task<IActionResult> VerifyCode(Verify_ViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var getCode = _myContext.Users.Where(U => U.SecurityStamp == userViewModel.SecurityStamp).Any();
                if (!getCode)
                {
                    return BadRequest(new { msg = "Verification proccess is failed. Please enter the invalid code" });
                }
                var userEmail = _myContext.UserRoles.Include("Role").Include("User").Where(U => U.User.Email == userViewModel.Email).FirstOrDefault();
                var getUser = new UsersViewModel();
                userEmail.User.SecurityStamp = null;
                userEmail.User.EmailConfirmed = true;
                getUser.RoleName = userEmail.Role.Name;
                getUser.Username = userEmail.User.UserName;
                getUser.Id = userEmail.User.Id;
                getUser.Email = userEmail.User.Email;
                getUser.Phone = userEmail.User.PhoneNumber;
                await _myContext.SaveChangesAsync();
                return StatusCode(200, getUser);
            }
            return BadRequest(500);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var pwd = userVM.Password;
                var masuk = _myContext.UserRoles.Include("Role").Include("User").FirstOrDefault(m => m.User.Email == userVM.Email);
                if (masuk == null)
                {
                    return BadRequest("Please use the existing email or register first");
                }
                else if (!BCrypt.Net.BCrypt.Verify(userVM.Password, masuk.User.PasswordHash))
                {
                    return BadRequest("Incorret password");
                }
                else if (pwd == null || pwd.Equals(""))
                {
                    return BadRequest("Please enter the password");
                }
                else
                {
                    var user = new UsersViewModel();
                    user.Id = masuk.User.Id;
                    user.Username = masuk.User.UserName;
                    user.Email = masuk.User.Email;
                    user.Phone = masuk.User.PhoneNumber;
                    user.RoleName = masuk.Role.Name;
                    if (user.Email != null)
                    {
                        var Claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim("id", user.Id),
                            new Claim("uname", user.Username),
                            new Claim("email", user.Email),
                            new Claim("phone", user.Phone),
                            new Claim("lvl", user.RoleName)
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], Claims, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                    }
                    return StatusCode(200, user);

                }
            }
            return BadRequest(500);
        }

    }
}
