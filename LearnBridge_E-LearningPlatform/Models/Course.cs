using System.ComponentModel.DataAnnotations;

namespace LearnBridge_E_LearningPlatform.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Level { get; set; } = "Beginner";

        // Foreign Key
        [Required]
        public int TeacherId { get; set; }

        // Navigation Property
        public Teacher? Teacher { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}