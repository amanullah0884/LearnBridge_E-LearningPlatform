using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  Admin & Teacher only
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Index()
        {
            var enrollments = await _context.Enrollments
                                    .Include(e => e.Student)
                                    .Include(e => e.Course)
                                    .ToListAsync();
            return View(enrollments);
        }

        //  Student only (Enroll)
        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Students = new SelectList(_context.Students.ToList(), "StudentId", "StudentName");
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title");
            ViewBag.Statuses = new SelectList(new List<string> { "Active" });
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                enrollment.Status = "Active";
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(_context.Students.ToList(), "StudentId", "StudentName", enrollment.StudentId);
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", enrollment.CourseId);
            return View(enrollment);
        }

        //  Admin & Teacher
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            ViewBag.Students = new SelectList(_context.Students.ToList(), "StudentId", "StudentName", enrollment.StudentId);
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", enrollment.CourseId);
            ViewBag.Statuses = new SelectList(
                new List<string> { "Active", "Completed", "Canceled" }, enrollment.Status);

            return View(enrollment);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(enrollment);
        }

        // Admin only
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (enrollment == null) return NotFound();
            return View(enrollment);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
