namespace Job_Portal.ViewModels
{
    public class LogInViewModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required bool RememberMe { get; set; } = false;
    }
}
