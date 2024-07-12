using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Job_Portal.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        [ForeignKey("CompanyId")]
        public required string CompanyId { get; set; }
        public required ApplicationUser Company { get; set; }
        
        public string JobTitle { get; set; } = string.Empty;
        public string JobCategory { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;

        public decimal HourPay { get; set; }
        public decimal MonthlyPay { get; set;}

        public int Applicants { get; set; } = 0;

    }
}
