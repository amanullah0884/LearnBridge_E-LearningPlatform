using Microsoft.AspNetCore.Identity;

namespace LearnBridge_E_LearningPlatform.Models
{
    public class ApplicationUser: IdentityUser

    {
        // Additional properties can be added here as needed
        public string? FullName { get; set; }
    }
}
