using Job_Portal.Models;
using Job_Portal.Repository.Interfaces;
using Job_Portal.Services.Interfaces;
using Job_Portal.ViewModels;

namespace Job_Portal.Services.Implementation
{
    public class JobService : IJobService
    {
        private readonly IJobRepository jobRepository;
        private readonly IApplicationUserRepository applicationUserRepository;

        public JobService(IJobRepository jobRepository , IApplicationUserRepository applicationUserRepository)
        {
            this.jobRepository = jobRepository;
            this.applicationUserRepository = applicationUserRepository;
        }

        public List<Job> GetJobs()
        {
            return jobRepository.GetAll();
        }
        public Job? GetJobById(int? jobId)
        {
            if(jobId == null)
            {
                return null;
            }else
            {
                return jobRepository.GetById(jobId);
            }
        }

        public bool CreatJob (JobViewModel model)
        {
            if (model != null) 
            {
                Job job = new Job();
                job.JobDescription = model.JobDescription;
                job.JobTitle = model.JobTitle;
                job.JobCategory = model.JobCategory;
                job.ClosingDate = model.ClosingDate;
                job.HourPay = model.HourPay;
                job.MonthlyPay = model.MonthlyPay;
                job.Company = applicationUserRepository.FindById(model.CompanyId);

                return jobRepository.CreatJob(job);
            }else
            {
                return false; 
            }
        }

        public bool DeleteJob (int id)
        {
            return jobRepository.DeleteById(id);
        }
    }
}
