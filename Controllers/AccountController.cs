using jwtAuthWebAPI.Data;
using jwtAuthWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace jwtAuthWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityDataContext _identityDataContext;

        public AccountController(JWTSettings jwtSettings, UserManager<ApplicationUser> userManager,IdentityDataContext identityDataContext)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _identityDataContext = identityDataContext;
        }
        [HttpGet("GetList")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetList()
        {
            return Ok("Done!");
        }
           
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(Users users)
        {
            try
            {
                var applicationUser = new ApplicationUser()
                {
                    UserName = users.UserName,
                    Email = users.EmailId,
                    FirstName = users.FirstName,
                    LastName = users.LastName,
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false
                };
                var result = await _userManager.CreateAsync(applicationUser, users.Password);
                if (result.Succeeded)
                {
                    return Ok("User Created");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }catch(Exception)
            {
                throw;
            }
        }
        
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken(UserLogins userLogins)
        {
            try
            {
                var Token = new UserTokens();
                var user = await _userManager.FindByNameAsync(userLogins.UserName);
                if (user == null)
                {
                    return BadRequest("User name is not valid");
                }
                var valid = await _userManager.CheckPasswordAsync(user, userLogins.Password);
                if(valid)
                {
                    var strToken = Guid.NewGuid().ToString();
                    var validity = DateTime.UtcNow.AddDays(15);
                    Token = JwtHelpers.JwtHelper.GenTokenKey(new UserTokens()
                    {
                        EmailId = user.Email,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Id = Guid.Parse(user.Id),
                        RefreshToken = strToken
                    }, _jwtSettings);
                    var tokenupdate = _identityDataContext.Users.Where(x => x.Id == user.Id).FirstOrDefault();
                    if (tokenupdate == null)
                    {
                        return BadRequest("User name is not valid");
                    }
                    tokenupdate.RefreshToken = strToken;
                    tokenupdate.RefreshTokenValidity = validity;
                    _identityDataContext.Update(tokenupdate);
                    _identityDataContext.SaveChanges();
                    Token.RefreshToken = strToken;
                 }
                else
                {
                    return BadRequest("Wrong password");
                }
                return Ok(Token);
            }catch (Exception )
            {
                throw;
            }
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel userLogins)
        {
            try
            {
                var Token = new UserTokens();
                var user = await _userManager.FindByNameAsync(userLogins.UserName);
                if(user == null)
                {
                    return BadRequest("User name is not valid");
                }
                var valid = _identityDataContext.Users.Where(x => x.UserName == userLogins.UserName
                && x.RefreshToken == userLogins.RefreshToken
                && x.RefreshTokenValidity > DateTime.UtcNow).Any();
                if (valid)
                {
                    var strToken = Guid.NewGuid().ToString();
                    var validity = DateTime.UtcNow.AddDays(15);
                    Token = JwtHelpers.JwtHelper.GenTokenKey(new UserTokens()
                    {
                        EmailId = user.Email,
                        GuidId = Guid.NewGuid(),
                        UserName = user.UserName,
                        Id = Guid.Parse(user.Id),
                        RefreshToken = strToken
                    }, _jwtSettings);
                    var tokenupdate = _identityDataContext.Users.Where(x => x.Id == user.Id).FirstOrDefault();
                    if (tokenupdate == null)
                    {
                        return BadRequest("User name is not valid");
                    }
                    tokenupdate.RefreshToken = strToken;
                    tokenupdate.RefreshTokenValidity = validity;
                    _identityDataContext.Update(tokenupdate);
                    _identityDataContext.SaveChanges();
                }
                else
                {
                    return BadRequest("Wrong Password");
                }
                return Ok(Token);

            }
            catch (Exception )
            {
                throw;
            }
        }

    }
}
