using System.ComponentModel.DataAnnotations;

namespace Job_Portal.ViewModels
{
    public class JobApplicationViewModel
    {
        public int JobId { get; set; }
        [Required]
        public IFormFile CVFile { get; set; }
    }
}
