using Job_Portal.Models;
using Job_Portal.Services.Interfaces;
using Job_Portal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;
using static System.Net.Mime.MediaTypeNames;

namespace Job_Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<JobApplicationController> logger;
        private readonly IEmailService emailService;

        public JobApplicationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, ILogger<JobApplicationController> logger, IEmailService emailService)
        {
            this.context = context;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this.emailService = emailService;
        }


        [HttpPost]
        public async Task<IActionResult> Apply([FromForm] JobApplicationViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var job = await context.Jobs
                .Include(j => j.Company) // Ensure Company is included
                .FirstOrDefaultAsync(j => j.Id == model.JobId);
            if (job == null)
            {
                return NotFound("Job not found.");
            }

            if (model.CVFile == null || model.CVFile.Length == 0)
            {
                return BadRequest("CV file is required.");
            }

            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
            string relativeFilePath = null;
            string absoluteFilePath = null; 

            if (model.CVFile != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CVFile.FileName;
                absoluteFilePath = Path.Combine(uploadsFolder, uniqueFileName);
                relativeFilePath = Path.Combine("uploads", uniqueFileName);

                try
                {
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(absoluteFilePath, FileMode.Create))
                    {
                        await model.CVFile.CopyToAsync(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while saving the file.");
                    return StatusCode(500, "Internal server error while saving the file.");
                }
            }

            var application = new JobApplication
            {
                JobId = model.JobId,
                ApplicationUserId = user.Id,
                AppliedDate = DateTime.Now,
                CVFilePath = relativeFilePath
            };

            context.JobApplications.Add(application);
            await context.SaveChangesAsync();

            var companyEmail = job.Company.Email; // Assuming you have an Email property in your Company entity
            var emailBody = $"A new application has been submitted for the job: {job.JobTitle}.";
            await emailService.SendEmailAsync(companyEmail, "New Job Application", emailBody, Path.Combine(webHostEnvironment.WebRootPath, absoluteFilePath));

            var applierEmail = user.Email;
            emailBody = $"Your application for the job: {job.JobTitle} has been submitted.\nand your application is currently :{application.Status}";
            await emailService.SendEmailAsync(applierEmail, "Your Job Application", emailBody);

            return Ok();
        }

        [HttpGet("api/jobapplication/myapplications")]
        [Authorize]

        public async Task<IActionResult> MyApplications()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var applications = await context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.ApplicationUserId == user.Id)
                .ToListAsync();

            return Ok(applications);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyJobApplications()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var applications = await context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.Job.CompanyId == user.Id)
                .ToListAsync();


            return Ok(applications);
        }

        [HttpPost]
        [Authorize]
        [Route("/api/Jobapplication/AcceptApplication/{id:int}")]
        
        public async Task<IActionResult> AcceptApplication(int id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("Not Authorized");
            }
            var application = context.JobApplications.Include(a => a.Job).Include(a => a.User).Single(x => x.Id == id);
            
            if (application == null)
            {
                return NotFound();
            }
            else
            {
                if (application.Job.CompanyId == user.Id)
                {
                    var applierEmail = application.User.Email;

                    application.Status = JobStatus.Accepted;


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
        [Authorize]
        [Route("/api/Jobapplication/RejectApplication/{id:int}")]


        public async Task<IActionResult> RejectApplication(int id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("Not Authorized");
            }

            var application = context.JobApplications.Include(a => a.Job).Include(a => a.User).Single(x => x.Id == id);

            if (application == null)
            {
                return NotFound();
            }
            else
            {
                if(application.Job.CompanyId == user.Id)
                {
                    var applierEmail = application.User.Email;

                    application.Status = JobStatus.Rejected;


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
