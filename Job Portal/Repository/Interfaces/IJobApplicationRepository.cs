using Job_Portal.Models;

namespace Job_Portal.Repository.Interfaces
{
    public interface IJobApplicationRepository
    {
        public bool SubmitApplication(JobApplication jobApplication);
        public List<JobApplication> GetUserApplications(string userId);
        public List<JobApplication> GetCompanyApplications(string companyId);

        public JobApplication GetJobApplication(int Id);

        public void AcceptJobApplication(JobApplication jobApplication);
        public void RejectJobApplication(JobApplication jobApplication);


    }
}
