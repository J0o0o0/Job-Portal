
using Job_Portal.Models;
using Job_Portal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal.Repository.Implementation
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext context;

        public JobApplicationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool SubmitApplication(JobApplication jobApplication)
        {
            try
            {
                context.JobApplications.Add(jobApplication);
                context.SaveChanges();

                var job = context.Jobs.Single(a => a.Id == jobApplication.JobId);
                job.Applicants += 1;
                context.Entry(job).State = EntityState.Modified;
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<JobApplication> GetUserApplications(string userId)
        {
            var applications = context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.ApplicationUserId == userId)
                .ToList();
            return applications;
        }

        public List<JobApplication> GetCompanyApplications(string companyId)
        {
            var applications = context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.Job.CompanyId == companyId)
                .Include(a => a.User)
                .ToList();
            return applications;
        }

        public JobApplication GetJobApplication(int Id)
        {
            var application = context.JobApplications.Include(a => a.Job).Include(a => a.User).Single(x => x.Id == Id);
            return application;
        }

        public void AcceptJobApplication(JobApplication jobApplication)
        {
            jobApplication.Status = JobStatus.Accepted;
            context.Entry(jobApplication).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void RejectJobApplication(JobApplication jobApplication)
        {
            jobApplication.Status = JobStatus.Rejected;
            context.Entry(jobApplication).State = EntityState.Modified;
            context.SaveChanges();
        }




    }
}
