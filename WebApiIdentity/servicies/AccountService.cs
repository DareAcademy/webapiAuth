using WebApiIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiIdentity.servicies
{
    public class AccountService: IAccountService
    {
        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;
        RoleManager<IdentityRole> roleManager;

        public AccountService(UserManager<ApplicationUser> _userManager,
                                SignInManager<ApplicationUser> _signInManager,
                                RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }


        public async Task<IdentityResult> Create(SignUpModel signUpModel)
        {
            ApplicationUser user = new ApplicationUser();
            user.Name = signUpModel.Name;
            user.UserName = signUpModel.Email;
            user.Email = signUpModel.Email;

           var result= await userManager.CreateAsync(user, signUpModel.Password);
            return result;
        }

        public async Task<SignInResult> SignIn(SignInModel signInModel)
        {
           var result= await signInManager.PasswordSignInAsync(signInModel.Username, signInModel.Password, signInModel.RememberMe, false);
            return result;
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> AddRole(RoleModel roleModel)
        {
            IdentityRole role = new IdentityRole();
            role.Name = roleModel.Name;

            var result= await roleManager.CreateAsync(role);
            return result;
        }

        public List<ApplicationUser> getUsers()
        {
            List<ApplicationUser> liUser= userManager.Users.ToList();
            return liUser;
        }

        public async Task<List<UserRoles>> getRoles(string UserId)
        {
            //List<IdentityRole> liRole=  roleManager.Roles.ToList();
            //  return liRole;
            List<UserRoles> liuserRole = new List<UserRoles>();
            List<IdentityRole> liRole = roleManager.Roles.ToList();
            foreach (IdentityRole item in liRole)
            {
                UserRoles uRole = new UserRoles();
                uRole.RoleId = item.Id;
                uRole.RoleName = item.Name;
                uRole.IsSelected = false;
                uRole.UserId =UserId;
                liuserRole.Add(uRole);
            }

            foreach (UserRoles item in liuserRole)
            {
                var user = await userManager.FindByIdAsync(item.UserId);

               var liRoleofUser= await userManager.GetRolesAsync(user);

                foreach (var Rolename in liRoleofUser)
                {
                    if (item.RoleName == Rolename)
                    {
                        item.IsSelected = true;
                    }
                }

            }

            return liuserRole;
        }

        public async Task<IdentityRole> getRole(string Id)
        {
           var result= await roleManager.FindByIdAsync(Id);
            return result;
        }

        public async Task<ApplicationUser> getUser(string username)
        {
            var result = await userManager.FindByNameAsync(username);
            return result;
        }



        public async Task UpdateUserRole(List<UserRoles> li)
        {
            
            foreach (var item in li)
            {
                var user = await userManager.FindByIdAsync(item.UserId);
                var role = await roleManager.FindByIdAsync(item.RoleId);

                if (item.IsSelected == true)
                {


                    if (await userManager.IsInRoleAsync(user, role.Name) == false)
                    {
                        // insert to UserRole

                        await userManager.AddToRoleAsync(user, role.Name);
                    }
                }
                else
                {
                    if (await userManager.IsInRoleAsync(user, role.Name)==true)
                    {
                        await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
            }
            
        }

        public List<string> getUserRoles(ApplicationUser obj)
        {
             List<string> li = userManager.GetRolesAsync(obj).Result.ToList();
            return li;
        }


    }
}
