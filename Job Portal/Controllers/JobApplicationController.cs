using Job_Portal.Models;
using Job_Portal.Services.Implementation;
using Job_Portal.Services.Interfaces;
using Job_Portal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobService jobService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly IJobApplicationService jobApplicationService;

        public JobApplicationController(IJobService jobService, UserManager<ApplicationUser> userManager, IEmailService emailService, IJobApplicationService jobApplicationService)
         {
            this.jobService = jobService;
            this.userManager = userManager;
            this.emailService = emailService;
            this.jobApplicationService = jobApplicationService;
        }

        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Apply([FromForm] JobApplicationViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Please Login.");
            }
            //model.ApplaierId = user.Id;

            var job = jobService.GetJobById(model.JobId);

            if (job == null)
            {
                return NotFound("Job not found.");
            }

            if (model.CVFile == null || model.CVFile.Length == 0)
            {
                return BadRequest("CV file is required.");
            }
            if (! await jobApplicationService.SubmitNewApplication(model, user.Id))
            {
                return BadRequest();
            }
            else
            {
                return Ok(model);
            }
        }

        [HttpGet("api/jobapplication/myapplications")]
        [Authorize(Roles = "JobSeeker")]

        public async Task<IActionResult> MyApplications()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(jobApplicationService.GetMyApplications(user.Id));
        }


        [HttpGet]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> MyJobApplications()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(jobApplicationService.GetMyJobApplications(user.Id));
        }


        [HttpPost]
        [Authorize(Roles = "Company")]
        [Route("/api/Jobapplication/AcceptApplication/{id:int}")]
        
        public async Task<IActionResult> AcceptApplication(int id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("Not Authorized");
            }
            var application = jobApplicationService.GetJobApplication(id);
            
            if (application == null)
            {
                return NotFound();
            }
            else
            {
                if (application.Job.CompanyId == user.Id)
                {
                    if(application.Status == JobStatus.Accepted)
                    {
                        return BadRequest("the application is already Accepted");
                    }
                    var applierEmail = application.User.Email;

                    jobApplicationService.AcceptJobApplication(application);


                    var emailBody = $"Hi {application.User.UserName}.\nYour application for the job: {application.Job.JobTitle} has been {application.Status}";
                    await emailService.SendEmailAsync(applierEmail, "Your Job Application", emailBody);

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }


        [HttpPost]
        [Authorize(Roles = "Company")]
        [Route("/api/Jobapplication/RejectApplication/{id:int}")]


        public async Task<IActionResult> RejectApplication(int id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("Not Authorized");
            }

            var application = jobApplicationService.GetJobApplication(id);

            if (application == null)
            {
                return NotFound();
            }
            else
            {
                if(application.Job.CompanyId == user.Id)
                {
                    if (application.Status == JobStatus.Rejected)
                    {
                        return BadRequest("the application is already Rejected");
                    }
                    var applierEmail = application.User.Email;

                    jobApplicationService.RejectJobApplication(application);


                    var emailBody = $"Hi {application.User.UserName}.\nYour application for the job: {application.Job.JobTitle} has been {application.Status}";
                    await emailService.SendEmailAsync(applierEmail, "Your Job Application", emailBody);

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        


    }
}
