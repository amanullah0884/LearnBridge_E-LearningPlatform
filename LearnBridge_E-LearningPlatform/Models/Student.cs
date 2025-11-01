using System.ComponentModel.DataAnnotations;

namespace LearnBridge_E_LearningPlatform.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required, MaxLength(100)]
        public String StudentName { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [MaxLength(200)]
        public string WeakSubjects { get; set; } = string.Empty;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}