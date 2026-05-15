using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _db;
        public StudentsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var students = await _db.Students.ToListAsync();
            return View(students);
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student is null) return NotFound();
            return View(student);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid) return View(student);
            student.EnrollmentDate = DateTime.UtcNow;
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student is null) return NotFound();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student updated)
        {
            if (id != updated.Id) return BadRequest();
            if (!ModelState.IsValid) return View(updated);

            var student = await _db.Students.FindAsync(id);
            if (student is null) return NotFound();

            student.FullName       = updated.FullName;
            student.Email          = updated.Email;
            student.EnrollmentDate = updated.EnrollmentDate;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student is null) return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student is null) return NotFound();
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
