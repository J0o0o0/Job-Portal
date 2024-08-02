
using Job_Portal.Models;
using Job_Portal.ViewModels;

namespace Job_Portal.Services.Interfaces
{
    public interface IJobService
    {
        public List<Job> GetJobs();
        public Task<Job>? GetJobById(int? id);
        public bool CreatJob (JobViewModel job, string userId);
        public bool DeleteJob (int id);
    }
}
