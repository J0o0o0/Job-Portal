using Job_Portal.Controllers;
using Job_Portal.Models;
using Job_Portal.Repository.Interfaces;
using Job_Portal.Services.Interfaces;
using Job_Portal.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Job_Portal.Services.Implementation
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<JobApplicationController> logger;
        private readonly IJobApplicationRepository jobApplicationRepository;
        private readonly IJobService jobService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IEmailService emailService;

        public JobApplicationService(IWebHostEnvironment webHostEnvironment, ILogger<JobApplicationController> logger, IJobApplicationRepository jobApplicationRepository, IJobService jobService, IApplicationUserService applicationUserService, IEmailService emailService)
        {

            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this.jobApplicationRepository = jobApplicationRepository;
            this.jobService = jobService;
            this.applicationUserService = applicationUserService;
            this.emailService = emailService;
        }


        public async Task<bool> SubmitNewApplication(JobApplicationViewModel model, string userId)
        {
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
                    return false;
                }
            }

            var application = new JobApplication
            {
                JobId = model.JobId,
                ApplicationUserId = userId,
                AppliedDate = DateTime.Now,
                CVFilePath = relativeFilePath,
                Job = await jobService.GetJobById(model.JobId),
                User = applicationUserService.FindById(userId)
            };

            if (jobApplicationRepository.SubmitApplication(application))
            {
                var companyEmail = application.Job.Company.Email; // Assuming you have an Email property in your Company entity
                var emailBody = $"A new application has been submitted for the job: {application.Job.JobTitle}.";
                await emailService.SendEmailAsync(companyEmail, "New Job Application", emailBody, Path.Combine(webHostEnvironment.WebRootPath, absoluteFilePath));

                var applierEmail = application.User.Email;
                emailBody = $"Your application for the job: {application.Job.JobTitle} has been submitted.\nand your application is currently :{application.Status}";
                await emailService.SendEmailAsync(applierEmail, "Your Job Application", emailBody);

                return true;
            }
            else
            {
                return false;
            }
        }

        public List<JobApplication> GetMyApplications(string userId) 
        {
            return jobApplicationRepository.GetUserApplications(userId);
        }

        public List<JobApplication> GetMyJobApplications(string companyId) 
        {
            return jobApplicationRepository.GetCompanyApplications(companyId);
        }
        public JobApplication GetJobApplication(int id)
        {
            var application = jobApplicationRepository.GetJobApplication(id);
            if (application == null)
            {
                return null;
            }
            else
            { return application; }
        }

        public void AcceptJobApplication(JobApplication jobApplication)
        {
            jobApplicationRepository.AcceptJobApplication(jobApplication);
        }
        public void RejectJobApplication(JobApplication jobApplication)
        {
            jobApplicationRepository.RejectJobApplication(jobApplication);
        }





    }
}
