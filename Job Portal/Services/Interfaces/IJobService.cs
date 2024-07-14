
using Job_Portal.Models;
using Job_Portal.ViewModels;

namespace Job_Portal.Services.Interfaces
{
    public interface IJobService
    {
        public List<Job> GetJobs();
        public Job GetJobById(int? id);
        public bool CreatJob (JobViewModel job);
        public bool DeleteJob (int id);
    }
}
