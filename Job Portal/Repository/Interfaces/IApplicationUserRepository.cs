using Job_Portal.Models;
using Job_Portal.ViewModels;

namespace Job_Portal.Repository.Interfaces
{
    public interface IApplicationUserRepository
    {
        public Task<bool> CreatUser(ApplicationUser user, string password, string role);
        public ApplicationUser FindById(string id);
        public Task<ApplicationUser> FindByName(string userName);
        public Task<bool> CheckPassword(ApplicationUser user, string password);
        public Task SignIn(ApplicationUser user ,bool RememberMe);
        public void SignOut();




    }
}
