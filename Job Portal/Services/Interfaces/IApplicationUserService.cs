using Job_Portal.ViewModels;
using Job_Portal.Models;
namespace Job_Portal.Services.Interfaces
{
    public interface IApplicationUserService
    {
        public ApplicationUser FindById(string id);
        public Task<bool> Register(ApplicationUserViewModel newUser);
        public Task<bool> LogIn(LogInViewModel userVM);
        public void LogOut();
    }
}
