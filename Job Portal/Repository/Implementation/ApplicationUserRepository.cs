using Job_Portal.Models;
using Job_Portal.ViewModel;
using Microsoft.AspNetCore.Identity;
using Job_Portal.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Job_Portal.ViewModels;

namespace Job_Portal.Repository.Implementation
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;

        public ApplicationUserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;
        }


        public async Task<bool> CreatUser(ApplicationUser user, string password, string role)
        {


            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
                return true;
            }
            else
            {
                return false;
            }

        }
        public ApplicationUser FindById(string id) 
        {
            if (id == null)
            {
                return null;
            }
            ApplicationUser user = context.ApplicationUsers.Single(x => x.Id.Equals(id));
            if(user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        public async Task<ApplicationUser> FindByName(string userName)
        {
            ApplicationUser? userModel = await userManager.FindByNameAsync(userName);

            return userModel;
        }

        public async Task<bool> CheckPassword(ApplicationUser user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task SignIn(ApplicationUser user, bool RememberMe)
        {
            await signInManager.SignInAsync(user, RememberMe);
        }
        public async void SignOut()
        {
           await signInManager.SignOutAsync();
        }


    }
}


