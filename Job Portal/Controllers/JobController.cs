using Job_Portal.Models;
using Job_Portal.Services.Implementation;
using Job_Portal.Services.Interfaces;
using Job_Portal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService jobService;

        public JobController(IJobService jobService)
        {
            this.jobService = jobService;
        }


        [HttpGet]
        public IActionResult Get()
        {            
            return Ok(jobService.GetJobs());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var job = jobService.GetJobById(id);
           if(job != null)
            {
                return Ok(job);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public IActionResult Post(JobViewModel model) 
        {
             
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(model == null) 
            {
                return BadRequest("please login");
            }
            if (jobService.CreatJob(model, userId ))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
        
        
    }
}
