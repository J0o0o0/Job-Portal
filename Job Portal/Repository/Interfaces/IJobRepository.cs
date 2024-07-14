using Job_Portal.Models;

namespace Job_Portal.Repository.Interfaces
{
    public interface IJobRepository
    {
        public List<Job> GetAll();
        public Job? GetById(int? id);
        public bool CreatJob (Job job);
        public bool DeleteById(int id);
        
    }
}
