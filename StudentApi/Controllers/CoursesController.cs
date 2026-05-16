using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;
        public CoursesController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var courses = await _db.Courses.ToListAsync();
            return View(courses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course is null) return NotFound();
            return View(course);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Name, string Description, int Credits, string Teacher)
        {
            var course = new Course
            {
                Name        = Name        ?? "",
                Description = Description ?? "",
                Credits     = Credits,
                Teacher     = Teacher     ?? ""
            };
            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course is null) return NotFound();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Name, string Description, int Credits, string Teacher)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course is null) return NotFound();

            course.Name        = Name        ?? "";
            course.Description = Description ?? "";
            course.Credits     = Credits;
            course.Teacher     = Teacher     ?? "";

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course is null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course is null) return NotFound();
            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
