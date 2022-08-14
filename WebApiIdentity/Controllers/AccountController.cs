using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiIdentity.Models;
using WebApiIdentity.servicies;

namespace WebApiIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountService accountService;
        IConfiguration configuration;

        public AccountController(IAccountService _accountService,IConfiguration _configuration)
        {
            accountService = _accountService;
            configuration = _configuration;
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task SignUp(SignUpModel signUpModel)
        {
            var result = await accountService.Create(signUpModel);
        }

        [HttpPost]
        [Route("NewRole")]
        public async Task NewRole(RoleModel model)
        {
            var result = await accountService.AddRole(model);
        }

        [HttpGet]
        [Route("UserList")]
        public List<ApplicationUser> UserList()
        {
            List<ApplicationUser> li = accountService.getUsers();
            return li;
            
        }

        [HttpGet]
        [Route("getUserUser")]
        public async Task<List<UserRoles>> getUserUser(string Id)
        {
            List<UserRoles> liRole = await accountService.getRoles(Id);

            return liRole;
        }

        [HttpPost]
        [Route("UpdateUserRole")]
        public async Task UpdateUserRole(List<UserRoles> liUserRole)
        {
            await accountService.UpdateUserRole(liUserRole);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            var result= await accountService.SignIn(signInModel);
            if (result.Succeeded)
            {
                //var authClaim = new List<Claim>();
                //Claim claim = new Claim("Email", signInModel.Username);
                //authClaim.Add(claim);
                //claim = new Claim("Today", DateTime.Now.ToString());
                //authClaim.Add(claim);

                var user = await accountService.getUser(signInModel.Username);


                var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, signInModel.Username),
                    new Claim("UniqueValue", Guid.NewGuid().ToString())
                };

                var roles = accountService.getUserRoles(user);

                foreach (var item in roles)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, item));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                            issuer: configuration["JWT:ValidIssuer"],
                            audience: configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddDays(15),
                            claims: authClaim,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });

                // build token
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
