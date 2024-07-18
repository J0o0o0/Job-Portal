using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Job_Portal.Models
{
    public class Job
    {
        public int Id { get; set; }
        [ForeignKey("CompanyId")]
        [Required]
        public string CompanyId { get; set; }
        [Required]
        public ApplicationUser Company { get; set; }
        
        public string JobTitle { get; set; } = string.Empty;
        public string JobCategory { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;

        public decimal HourPay { get; set; }
        public decimal MonthlyPay { get; set;}

        public int Applicants { get; set; } = 0;

        [Required]
        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ClosingDate { get; set; }
        [JsonIgnore]
        public ICollection<JobApplication> JobApplications { get; set;}

    }
}
