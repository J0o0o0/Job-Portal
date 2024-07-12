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
        private readonly IRepositoryManger repositoryManger;

        public ApplicationUserService(IRepositoryManger repositoryManger)
        {
            this.repositoryManger = repositoryManger;
        }

        public async Task<bool> Register(ApplicationUserViewModel newUser)
        {
            if (newUser.Role == "Company" || newUser.Role == "JopSeeker")
            { 
                ApplicationUser user = new ApplicationUser();
                user.UserName = newUser.UserName;
                user.Email = newUser.Email;
                user.PasswordHash = newUser.Password;
                user.PhoneNumber = newUser.PhoneNumber;
                user.Address = newUser.Address;

                bool registered = await repositoryManger.UserRepository.CreatUser(user, newUser.Password, newUser.Role);
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
            ApplicationUser? userModel = await repositoryManger.UserRepository.FindByName(userVM.UserName);
            if (userModel != null)
            {
                bool loggedIn = await repositoryManger.UserRepository.CheckPassword(userModel ,userVM.Password);
                if (loggedIn)
                {
                    repositoryManger.UserRepository.SignIn(userModel , userVM.RememberMe);
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
            repositoryManger.UserRepository.SignOut();
        }
    }

}

