using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Course List
        public IActionResult Index()
        {
            var courses = _context.Courses
                .Include(c => c.Teacher)
                .ToList();

            return View(courses);
        }

        // Course Details
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        // Create Course
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult Create()
        {
            ViewBag.Teachers = _context.Teachers.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teachers = _context.Teachers.ToList();
            return View(course);
        }

        // Edit Course
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.Find(id);

            if (course == null)
                return NotFound();

            ViewBag.Teachers = _context.Teachers.ToList();

            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult Edit(int id, Course course)
        {
            if (id != course.CourseId)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teachers = _context.Teachers.ToList();
            return View(course);
        }

        // Delete
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.Find(id);

            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}