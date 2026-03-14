using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lesson List
        public async Task<IActionResult> Index()
        {
            var lessons = await _context.Lessons
                .Include(l => l.Course)
                .ToListAsync();

            return View(lessons);
        }



        // Create GET
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "Title");
            return View();
        }

        // Create POST
        [HttpPost]
        
        public async Task<IActionResult> Create(Lesson lesson, IFormFile MaterialUpload)
        {
            if (ModelState.IsValid)
            {
                // File Upload
                if (MaterialUpload != null && MaterialUpload.Length > 0)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/materials");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MaterialUpload.FileName);

                    var filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await MaterialUpload.CopyToAsync(stream);
                    }

                    lesson.MaterialFile = fileName;
                }

                _context.Add(lesson);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "Title", lesson.CourseId);
            return View(lesson);
        }

        // Edit GET
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "Title", lesson.CourseId);

            return View(lesson);
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lesson lesson, IFormFile MaterialUpload)
        {
            if (id != lesson.LessonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (MaterialUpload != null && MaterialUpload.Length > 0)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/materials");

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MaterialUpload.FileName);

                    var filePath = Path.Combine(uploadFolder, fileName);

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

            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "Title", lesson.CourseId);

            return View(lesson);
        }

        // Delete GET
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(m => m.LessonId == id);

            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // Delete POST
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
        // Lesson Details
        public async Task<IActionResult> Details(int id)
        {
            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(l => l.LessonId == id);

            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }
    }
}