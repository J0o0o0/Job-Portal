using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Job_Portal.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions options):base(options) 
        { 

        }
        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<JobApplication>().HasOne(p => p.Job).WithMany(p => p.JobApplications).HasForeignKey(p => p.JobId).OnDelete(DeleteBehavior.NoAction);

        }
    }

}
    

