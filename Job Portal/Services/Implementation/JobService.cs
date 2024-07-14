using Job_Portal.Models;
using Job_Portal.Repository.Interfaces;
using Job_Portal.Services.Interfaces;
using Job_Portal.ViewModels;

namespace Job_Portal.Services.Implementation
{
    public class JobService : IJobService
    {
        private readonly IRepositoryManger repositoryManger;

        public JobService(IRepositoryManger repositoryManger)
        {
            this.repositoryManger = repositoryManger;
        }

        public List<Job> GetJobs()
        {
            return repositoryManger.JobRepository.GetAll();
        }
        public Job GetJobById(int? jobId)
        {
            if(jobId == null)
            {
                return null;
            }else
            {
                return repositoryManger.JobRepository.GetById(jobId);
            }
        }

        public bool CreatJob (JobViewModel model)
        {
            if (model != null) 
            {
                Job job = new Job();
                job.JobDescription = model.JobDescription;
                job.JobCategory = model.JobCategory;
                job.ClosingDate = model.ClosingDate;
                job.HourPay = model.HourPay;
                job.MonthlyPay = model.MonthlyPay;
                job.Company = repositoryManger.UserRepository.FindById(model.CompanyId);

                return repositoryManger.JobRepository.CreatJob(job);
            }else
            {
                return false; 
            }
        }

        public bool DeleteJob (int id)
        {
            return repositoryManger.JobRepository.DeleteById(id);
        }
    }
}
