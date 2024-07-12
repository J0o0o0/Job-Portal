using Job_Portal.ViewModel;
using Job_Portal.ViewModels;

namespace Job_Portal.Services.Interfaces
{
    public interface IApplicationUserService
    {
        public Task<bool> Register(ApplicationUserViewModel newUser);
        public Task<bool> LogIn(LogInViewModel userVM);
        public void LogOut();
    }
}
