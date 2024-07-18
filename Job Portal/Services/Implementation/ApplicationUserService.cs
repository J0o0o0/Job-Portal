using Job_Portal.Models;
using Job_Portal.Repository.Interfaces;
using Job_Portal.Services.Interfaces;
using Job_Portal.ViewModel;
using Job_Portal.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Job_Portal.Services.Implementation
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository applicationUserRepository;

        public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
        }

        public async Task<bool> Register(ApplicationUserViewModel newUser)
        {
            if (newUser.Role == "Company" || newUser.Role == "JobSeeker")
            { 
                ApplicationUser user = new ApplicationUser();
                user.UserName = newUser.UserName;
                user.Email = newUser.Email;
                user.PasswordHash = newUser.Password;
                user.PhoneNumber = newUser.PhoneNumber;
                user.Address = newUser.Address;

                bool registered = await applicationUserRepository.CreatUser(user, newUser.Password, newUser.Role);
                if (registered)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> LogIn(LogInViewModel userVM)
        {
            ApplicationUser? userModel = await applicationUserRepository.FindByName(userVM.UserName);
            if (userModel != null)
            {
                bool loggedIn = await applicationUserRepository.CheckPassword(userModel ,userVM.Password);
                if (loggedIn)
                {
                    await applicationUserRepository.SignIn(userModel , userVM.RememberMe);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void LogOut()
        {
            applicationUserRepository.SignOut();
        }
    }

}

