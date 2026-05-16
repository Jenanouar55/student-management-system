using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    public class GradesController : Controller
    {
        private readonly AppDbContext _db;
        public GradesController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var grades = await _db.Grades
                .Include(g => g.Student)
                .Include(g => g.Course)
                .OrderByDescending(g => g.Date)
                .ToListAsync();
            return View(grades);
        }

        public async Task<IActionResult> Details(int id)
        {
            var grade = await _db.Grades
                .Include(g => g.Student)
                .Include(g => g.Course)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (grade is null) return NotFound();
            return View(grade);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _db.Students.ToListAsync(), "Id", "FullName");
            ViewBag.Courses  = new SelectList(await _db.Courses.ToListAsync(),  "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grade grade)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Students = new SelectList(await _db.Students.ToListAsync(), "Id", "FullName");
                ViewBag.Courses  = new SelectList(await _db.Courses.ToListAsync(),  "Id", "Name");
                return View(grade);
            }
            grade.Date = DateTime.UtcNow;
            _db.Grades.Add(grade);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var grade = await _db.Grades.FindAsync(id);
            if (grade is null) return NotFound();
            ViewBag.Students = new SelectList(await _db.Students.ToListAsync(), "Id", "FullName", grade.StudentId);
            ViewBag.Courses  = new SelectList(await _db.Courses.ToListAsync(),  "Id", "Name",     grade.CourseId);
            return View(grade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grade updated)
        {
            if (id != updated.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Students = new SelectList(await _db.Students.ToListAsync(), "Id", "FullName");
                ViewBag.Courses  = new SelectList(await _db.Courses.ToListAsync(),  "Id", "Name");
                return View(updated);
            }

            var grade = await _db.Grades.FindAsync(id);
            if (grade is null) return NotFound();

            grade.StudentId = updated.StudentId;
            grade.CourseId  = updated.CourseId;
            grade.Score     = updated.Score;
            grade.Note      = updated.Note;
            grade.Date      = updated.Date;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var grade = await _db.Grades
                .Include(g => g.Student)
                .Include(g => g.Course)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (grade is null) return NotFound();
            return View(grade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _db.Grades.FindAsync(id);
            if (grade is null) return NotFound();
            _db.Grades.Remove(grade);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
