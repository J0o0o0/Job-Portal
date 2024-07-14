namespace Job_Portal.Repository.Interfaces
{
    public class IRepositoryManger
    {
        public IApplicationUserRepository UserRepository { get; set; }
        public IJobRepository JobRepository { get; set; }
    }
}
