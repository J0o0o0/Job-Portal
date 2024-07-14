using Job_Portal.Models;
using Job_Portal.Services.Implementation;
using Job_Portal.Services.Interfaces;
using Job_Portal.ViewModels;
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
        public IActionResult Post(JobViewModel model) 
        {
            model.CompanyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (jobService.CreatJob(model))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            if (jobService.DeleteJob(id))
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
