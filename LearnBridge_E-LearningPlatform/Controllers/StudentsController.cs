using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var students = _context.Students.ToList();
            return View(students);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.Users.ToList(), "UserId", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = new SelectList(_context.Users.ToList(), "UserId", "Name", student.UserId);
            return View(student);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return NotFound();
           ViewBag.Users = new SelectList(_context.Users.ToList(),"userId","Name", student.UserId);
            return View(student);
        }
        [HttpPost]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.StudentId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Students.Update(student);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = new SelectList(_context.Users.ToList(), "UserId", "Name", student.UserId);
            return View(student);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
