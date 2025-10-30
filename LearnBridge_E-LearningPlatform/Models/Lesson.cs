using System.ComponentModel.DataAnnotations;

namespace LearnBridge_E_LearningPlatform.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }

        [Required, MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        public required string Content { get; set; }

        [MaxLength(500)]
        public string VideoUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string MaterialFile { get; set; } = string.Empty;

        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
