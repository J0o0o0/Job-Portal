using Job_Portal.Models;
using Job_Portal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public JobApplicationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Apply(JobApplicationViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var job = await context.Jobs.FindAsync(model.JobId);
            if (job == null)
            {
                return NotFound();
            }

            string filePath = null;
            if (model.CVFile != null)
            {
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CVFile.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CVFile.CopyToAsync(fileStream);
                }

                filePath = "/uploads/" + uniqueFileName;
            }

            var application = new JobApplication
            {
                JobId = model.JobId,
                ApplicationUserId = user.Id,
                AppliedDate = DateTime.Now,
                CVFilePath = filePath
            };

            context.JobApplications.Add(application);
            await context.SaveChangesAsync();

            return Ok();
        }

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
    }
}
