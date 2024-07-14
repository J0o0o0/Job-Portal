using Job_Portal.Services.Interfaces;
using Job_Portal.ViewModel;
using Job_Portal.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService applicationUserService;

        public ApplicationUserController(IApplicationUserService applicationUserService)
        {
            this.applicationUserService = applicationUserService;
        }
        
        [HttpPost]
        [Route("/api/applicationuser/register")]
        public async Task <IActionResult> Register(ApplicationUserViewModel model)
        {

           if (ModelState.IsValid)
            {

            if (await applicationUserService.Register(model))
            {
                
                return Ok(ModelState);
            }
            else
            {
                return BadRequest(ModelState);
            }

            }
            else { return BadRequest(ModelState); }

        }

        [HttpPost]
        [Route("/api/applicationuser/login")]
        public async Task<IActionResult> LogIn (LogInViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(await applicationUserService.LogIn(model))
                {
                    return Ok(ModelState);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("/api/applicationuser/logout")]

        public void LogOut ()
        {
            applicationUserService.LogOut();
        }
    }
}
