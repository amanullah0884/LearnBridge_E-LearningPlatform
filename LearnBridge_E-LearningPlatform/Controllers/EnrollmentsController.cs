using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin & Teacher ALL ENROLLMENTS
  
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Index()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.ApplicationUser)
                .ToListAsync();

            return View(enrollments);
        }

        // STUDENT  CREATE ENROLLMENT

        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "Title");
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //  Duplicate enrollment prevent
            bool alreadyEnrolled = await _context.Enrollments.AnyAsync(e =>
                e.ApplicationUserId == userId &&
                e.CourseId == enrollment.CourseId);

            if (alreadyEnrolled)
            {
                ModelState.AddModelError("", "You are already enrolled in this course.");
            }

            if (ModelState.IsValid)
            {
                enrollment.ApplicationUserId = userId;
                enrollment.Status = "Active";
                enrollment.EnrolledAt = DateTime.Now;

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyEnrollments));
            }

            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "Title", enrollment.CourseId);
            return View(enrollment);
        }

      
        // STUDENT  OWN ENROLLMENTS
   
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyEnrollments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var myEnrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.ApplicationUserId == userId)
                .ToListAsync();

            return View(myEnrollments);
        }

        // ADMIN & TEACHER  EDIT STATUS
  
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            ViewBag.Statuses = new SelectList(
                new List<string> { "Active", "Completed", "Canceled" },
                enrollment.Status);

            return View(enrollment);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId) return NotFound();

            //  Ownership protect
            var existing = await _context.Enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);

            if (existing == null) return NotFound();

            enrollment.ApplicationUserId = existing.ApplicationUserId;
            enrollment.EnrolledAt = existing.EnrolledAt;
            enrollment.CourseId = existing.CourseId;

            if (ModelState.IsValid)
            {
                _context.Update(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(enrollment);
        }


        // ADMIN  DELETE
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _context.Enrollments
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
