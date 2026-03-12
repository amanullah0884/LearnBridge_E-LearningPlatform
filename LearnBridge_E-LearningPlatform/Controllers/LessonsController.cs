using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // For IFormFile
using System.IO;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lessons = await _context.Lessons.Include(l => l.Course).ToListAsync();
            return View(lessons);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lesson lesson, IFormFile MaterialUpload)
        {
            if (ModelState.IsValid)
            {
                // File upload handle
                if (MaterialUpload != null && MaterialUpload.Length > 0)
                {
                    var fileName = Path.GetFileName(MaterialUpload.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/materials", fileName);
                   

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await MaterialUpload.CopyToAsync(stream);
                    }

                    lesson.MaterialFile = fileName;
                }

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", lesson.CourseId);
            return View(lesson);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return NotFound();

            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", lesson.CourseId);
            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lesson lesson, IFormFile MaterialUpload)
        {
            if (id != lesson.LessonId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // File upload handle
                    if (MaterialUpload != null && MaterialUpload.Length > 0)
                    {
                        var fileName = Path.GetFileName(MaterialUpload.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/materials", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await MaterialUpload.CopyToAsync(stream);
                        }

                        lesson.MaterialFile = fileName;
                    }

                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.LessonId)) return NotFound();
                    else throw;
                }
            }

            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", lesson.CourseId);
            return View(lesson);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(m => m.LessonId == id);

            if (lesson == null) return NotFound();

            return View(lesson);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.LessonId == id);
        }
    }
}
