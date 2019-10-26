using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Booksy.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDB.IdentityUser;
using Booksy.Settings;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Booksy.Models;
using Booksy.Helpers;
using AutoMapper;

namespace Booksy.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private UserManager<ApplicationUser> UserManager;
        private IConfiguration _config;
        IMapper Mapper;
        public UsersController(
            UserManager<ApplicationUser> userMagaer,
            IConfiguration config,
            IMapper mapper)
        {
            UserManager = userMagaer;
            _config = config;
            Mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserDto userDto)
        {
            var user = await UserManager.FindByNameAsync(userDto.UserName);
            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, userDto.Password);
            if (result != PasswordVerificationResult.Success)
                return BadRequest(new { message = "Username or password is incorrect" });

            var token = BuildToken(user);
            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = "mustafa",
                LastName = "kenedy",
                Token = token
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserDto userDto)
        {
            try
            {
                // save 
                ApplicationUser user = new ApplicationUser();
                user.UserName = userDto.UserName;
                //user.PhoneNumber = userDto.UserName;
                user.Email = userDto.Email;
                user.Location = GeoJsonHelper.GetJsonPoint(userDto.Location.Latitude, userDto.Location.Longitude);
                IdentityResult result = await UserManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                else
                {
                    return Ok(result.Errors);
                }

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var userDto = Mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        private string BuildToken(ApplicationUser user)
        {
            var u = UserManager.Users.FirstOrDefault(x => x.Email == user.Email);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,u.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:JwtAudience"],
              expires: DateTime.Now.AddDays(30),
              signingCredentials: creds,
              claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}