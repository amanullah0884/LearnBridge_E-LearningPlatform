using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeachersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var teacher = _context.Teachers.ToList();
            return View(teacher);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users.ToList(), "UserId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Teachers.Add(teacher);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = new SelectList(_context.Users.ToList(), "UserId", "Name", teacher.UserId);
            return View(teacher);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher == null) return NotFound();

            ViewBag.Users = new SelectList(_context.Users.ToList(), "UserId", "Name", teacher.UserId);
            return View(teacher);
        }

        [HttpPost]
        public IActionResult Edit(int id, Teacher teacher)
        {
            if (id != teacher.TeacherId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Teachers.Update(teacher);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = new SelectList(_context.Users.ToList(), "UserId", "Name", teacher.UserId);
            return View(teacher);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
