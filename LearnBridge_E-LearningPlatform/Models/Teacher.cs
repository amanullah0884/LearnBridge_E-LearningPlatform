using System.ComponentModel.DataAnnotations;

namespace LearnBridge_E_LearningPlatform.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [MaxLength(200)]
        public string Expertise { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Bio { get; set; } = string.Empty;

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}