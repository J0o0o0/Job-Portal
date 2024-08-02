using Job_Portal.ViewModels;
using Job_Portal.Models;

namespace Job_Portal.Services.Interfaces
{
    public interface IJobApplicationService
    {
        public Task<bool> SubmitNewApplication(JobApplicationViewModel model, string userId);
        public List<JobApplication> GetMyApplications(string userId);
        public List<JobApplication> GetMyJobApplications(string companyId);
        public JobApplication GetJobApplication(int id);

        public void AcceptJobApplication(JobApplication jobApplication);
        public void RejectJobApplication(JobApplication jobApplication);


    }
}
