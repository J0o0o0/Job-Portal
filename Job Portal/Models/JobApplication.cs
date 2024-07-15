using System.ComponentModel.DataAnnotations;

namespace Job_Portal.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        [Required]
        public int JobId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public DateTime AppliedDate { get; set; }
        public string CVFilePath { get; set; }


        public Job Job { get; set; }
        public ApplicationUser User { get; set; }
    }
}
