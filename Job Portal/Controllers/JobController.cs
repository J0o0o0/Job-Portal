using Job_Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public JobController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public IActionResult get()
        {
            
            
            return Ok();
        }
    }
}
