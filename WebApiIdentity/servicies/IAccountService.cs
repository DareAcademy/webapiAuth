using WebApiIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiIdentity.servicies
{
    public interface IAccountService
    {
        Task<IdentityResult> Create(SignUpModel signUpModel);
        Task<SignInResult> SignIn(SignInModel signInModel);

        Task Logout();

        Task<IdentityResult> AddRole(RoleModel roleModel);

        List<ApplicationUser> getUsers();

        Task<List<UserRoles>> getRoles(string UserId);

        Task UpdateUserRole(List<UserRoles> li);

        Task<IdentityRole> getRole(string Id);
        Task<ApplicationUser> getUser(string username);

        List<string> getUserRoles(ApplicationUser obj);
    }
}
