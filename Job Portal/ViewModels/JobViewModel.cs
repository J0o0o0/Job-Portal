using Job_Portal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Job_Portal.ViewModels
{
    public class JobViewModel
    {
        
        [ForeignKey("CompanyId")]

        public string JobTitle { get; set; } = string.Empty;
        public string JobCategory { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;

        public decimal HourPay { get; set; }
        public decimal MonthlyPay { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime ClosingDate { get; set; }
    }
}
