using Job_Portal.Models;
using Job_Portal.Repository.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal.Repository.Implementation
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext context;

        public JobRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<Job> GetAll()
        {
            return context.Jobs.Where(x => x.ClosingDate > DateTime.Now).ToList();
        }

        public async Task<Job>? GetById(int? id)
        {
            var job = await context.Jobs
                .Include(j => j.Company) 
                .FirstOrDefaultAsync(j => j.Id == id);
            if (job == null)
            {
                return null;
            }
            else
            {
                return job;
            }

        }

        public bool CreatJob(Job job)
        {
            if(job != null)
            {
                job.PostedDate = DateTime.Now;
                context.Jobs.Add(job);
                context.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }

        public bool DeleteById(int id)
        {
            
            var job =  context.Jobs.Find(id);
            if (job != null)
            {
                context.Jobs.Remove(job);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
