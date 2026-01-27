using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AdminController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IActionResult> Dashboard()
        {
            var model= new AdminDashboardViewModel
            {
    
                TotalTeachers = await _applicationDbContext.Teachers.CountAsync(),
                TotalStudents = await _applicationDbContext.Students.CountAsync(),
                TotalCourses = await _applicationDbContext.Courses.CountAsync(),
                TotalEnrollments = await _applicationDbContext.Enrollments.CountAsync(),

                 RecentEnrollments = await _applicationDbContext.Enrollments
                    .Include(e => e.Course)
                    .OrderByDescending(e => e.EnrolledAt)
                    .Take(5)
                    .ToListAsync()
            };
            return View(model);
        }
    }
}
