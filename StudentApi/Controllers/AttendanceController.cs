using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    /// <summary>
    /// REST API controller for managing attendance records.
    /// All routes are prefixed with /api/attendance.
    ///
    /// Endpoints:
    ///   GET    /api/attendance                  → list all records (with student name)
    ///   GET    /api/attendance/student/{id}     → list records for one student
    ///   POST   /api/attendance                  → add a new record
    ///   PUT    /api/attendance/{id}             → update a record
    ///   DELETE /api/attendance/{id}             → delete a record
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AttendanceController(AppDbContext db) => _db = db;

        /// <summary>
        /// GET /api/attendance
        /// Returns all attendance records joined with the student name.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<object>> GetAll() =>
            await _db.Attendances
                .Include(a => a.Student)
                .Select(a => new {
                    a.Id,
                    a.StudentId,
                    studentName = a.Student!.FullName,
                    a.Date,
                    a.Status,
                    a.Remark
                })
                .OrderByDescending(a => a.Date)
                .ToListAsync();

        /// <summary>
        /// GET /api/attendance/student/{studentId}
        /// Returns attendance records for a single student.
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<IEnumerable<Attendance>> GetByStudent(int studentId) =>
            await _db.Attendances
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

        /// <summary>
        /// POST /api/attendance
        /// Creates a new attendance record.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(Attendance a)
        {
            _db.Attendances.Add(a);
            await _db.SaveChangesAsync();
            return Ok(a);
        }

        /// <summary>
        /// PUT /api/attendance/{id}
        /// Updates the student, date, status, and remark of a record.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Attendance updated)
        {
            var a = await _db.Attendances.FindAsync(id);
            if (a is null) return NotFound();
            a.StudentId = updated.StudentId;
            a.Date      = updated.Date;
            a.Status    = updated.Status;
            a.Remark    = updated.Remark;
            await _db.SaveChangesAsync();
            return Ok(a);
        }

        /// <summary>
        /// DELETE /api/attendance/{id}
        /// Removes an attendance record permanently.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.Attendances.FindAsync(id);
            if (a is null) return NotFound();
            _db.Attendances.Remove(a);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
