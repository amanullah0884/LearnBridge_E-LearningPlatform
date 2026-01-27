namespace LearnBridge_E_LearningPlatform.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalCourses { get; set; }
        public int TotalEnrollments { get; set; }

        public List<Enrollment> RecentEnrollments { get; set; }
    }
}
