using Microsoft.EntityFrameworkCore;
using System;
using System;
using System.ComponentModel.DataAnnotations;

namespace LearnBridge_E_LearningPlatform.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required string Role { get; set; } // Admin, Teacher, Student

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
    }
}

