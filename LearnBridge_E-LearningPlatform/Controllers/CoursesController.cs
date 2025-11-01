using LearnBridge_E_LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace LearnBridge_E_LearningPlatform.Controllers
{
    
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Course course)
        {
            if(ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }
        [HttpGet]
        public  IActionResult Edit(int id)
        {
            var  couse =_context.Courses.Find(id);
            if(couse == null)
            {
                return NotFound();
            }
            return View(couse);
        }
        [HttpPost]
        public IActionResult Edit(int id, Course course)
        {
            if (id != course.CourseId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }
        public IActionResult Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();
            return View(course); 
        }

       
        [HttpPost, ActionName("Delete")]
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
