using System.ComponentModel.DataAnnotations;

namespace LearnBridge_E_LearningPlatform.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required, MaxLength(200)]
        public required string Title { get; set; }

        [Required, MaxLength(1000)]
        public required string Description { get; set; }

        [MaxLength(50)]
        public string Level { get; set; } = "Beginner"; // Beginner, Intermediate, Advanced

        [Required]
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
