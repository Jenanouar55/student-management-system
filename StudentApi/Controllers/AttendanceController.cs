using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _db;
        public AttendanceController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var records = await _db.Attendances
                .Include(a => a.Student)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
            return View(records);
        }

        public async Task<IActionResult> Details(int id)
        {
            var record = await _db.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (record is null) return NotFound();
            return View(record);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Students = new SelectList(await _db.Students.ToListAsync(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Students = new SelectList(await _db.Students.ToListAsync(), "Id", "FullName");
                return View(attendance);
            }
            _db.Attendances.Add(attendance);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var record = await _db.Attendances.FindAsync(id);
            if (record is null) return NotFound();
            ViewBag.Students = new SelectList(await _db.Students.ToListAsync(), "Id", "FullName", record.StudentId);
            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Attendance updated)
        {
            if (id != updated.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Students = new SelectList(await _db.Students.ToListAsync(), "Id", "FullName");
                return View(updated);
            }

            var record = await _db.Attendances.FindAsync(id);
            if (record is null) return NotFound();

            record.StudentId = updated.StudentId;
            record.Date      = updated.Date;
            record.Status    = updated.Status;
            record.Remark    = updated.Remark;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var record = await _db.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (record is null) return NotFound();
            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record = await _db.Attendances.FindAsync(id);
            if (record is null) return NotFound();
            _db.Attendances.Remove(record);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
