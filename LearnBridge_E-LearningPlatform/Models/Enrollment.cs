using System.ComponentModel.DataAnnotations;

namespace LearnBridge_E_LearningPlatform.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        [Required]
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.Now;

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Active"; // Active, Completed, Canceled
    }
}
