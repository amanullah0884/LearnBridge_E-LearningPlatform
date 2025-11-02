using LearnBridge_E_LearningPlatform.Models;
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


        public async Task<IActionResult> Index()
        {
            var enrollments = await _context.Enrollments
                                    .Include(e => e.Student)
                                    .Include(e => e.Course)
                                    .ToListAsync();
            return View(enrollments);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Students = new SelectList(_context.Students.ToList(), "StudentId", "StudentName");
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title");
            ViewBag.Statuses = new SelectList(new List<string> { "Active", "Completed", "Canceled" });
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(_context.Students.ToList(), "StudentId", "StudentName", enrollment.StudentId);
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", enrollment.CourseId);
            ViewBag.Statuses = new SelectList(new List<string> { "Active", "Completed", "Canceled" }, enrollment.Status);
            return View(enrollment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            ViewBag.Students = new SelectList(_context.Students.ToList(), "StudentId", "StudentName", enrollment.StudentId);
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", enrollment.CourseId);
            ViewBag.Statuses = new SelectList(new List<string> { "Active", "Completed", "Canceled" }, enrollment.Status);

            return View(enrollment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.EnrollmentId)) return NotFound();
                    else throw;
                }
            }

            ViewBag.Students = new SelectList(_context.Students.ToList(), "StudentId", "StudentName", enrollment.StudentId);
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", enrollment.CourseId);
            ViewBag.Statuses = new SelectList(new List<string> { "Active", "Completed", "Canceled" }, enrollment.Status);
            return View(enrollment);
        }

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

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentId == id);
        }
    }
}
